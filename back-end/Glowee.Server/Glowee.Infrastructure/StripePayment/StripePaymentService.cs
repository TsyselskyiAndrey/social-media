using Glowee.Application.Contracts.Identity;
using Glowee.Application.Contracts.Logging;
using Glowee.Application.Contracts.Persistence;
using Glowee.Application.Contracts.StripePayment;
using Glowee.Application.Exceptions;
using Glowee.Application.Models.StripePayment;
using Glowee.Domain.Entities.Users;
using Glowee.Domain.Entities.UserSubscriptions;
using Microsoft.Extensions.Options;
using Stripe;
using Stripe.Checkout;
using UnauthorizedAccessException = Glowee.Application.Exceptions.UnauthorizedAccessException;

namespace Glowee.Infrastructure.StripePayment
{
    public class StripePaymentService : IStripePaymentService
    {
        private readonly IUserService _userService;
        private readonly StripePaymentSettings _stripePaymentSettings;
        private readonly IUserRepository _userRepository;
        private readonly ISubscriptionRepository _subscriptionRepository;
        private readonly IUserSubscriptionRepository _userSubscriptionRepository;
        private readonly IAppLogger<StripePaymentService> _appLogger;

        public StripePaymentService(IOptions<StripePaymentSettings> stripePaymentSettings, IUserService userService, IUserRepository userRepository, ISubscriptionRepository subscriptionRepository, IUserSubscriptionRepository userSubscriptionRepository, IAppLogger<StripePaymentService> appLogger)
        {
            _stripePaymentSettings = stripePaymentSettings.Value;
            _userService = userService;
            _userRepository = userRepository;
            _subscriptionRepository = subscriptionRepository;
            _userSubscriptionRepository = userSubscriptionRepository;
            _appLogger = appLogger;
            StripeConfiguration.ApiKey = _stripePaymentSettings.SecretKey;
        }

        public string Config()
        {
            return _stripePaymentSettings.PublishableKey;
        }

        public async Task<CreateCheckoutSessionResponse> CreateCheckoutSessionAsync(string priceId)
        {
            if (string.IsNullOrEmpty(_userService.UserId))
                throw new UnauthorizedAccessException("User must be authenticated to create a checkout session.");

            var userId = long.Parse(_userService.UserId);
            var user = await _userRepository.GetByIdAsync(new UserId(userId));

            if (user == null)
                throw new BadRequestException("User not found.");

            var existingSubscriptions = await _userSubscriptionRepository.GetByUserIdAsync(user.Id);

            var hasActivePlan = existingSubscriptions.Any(us => us.IsActive);

            if (hasActivePlan)
                throw new BadRequestException("You already have an active subscription.");

            if (string.IsNullOrEmpty(user.StripeCustomerId))
            {
                var customerService = new CustomerService();
                var stripeCustomer = await customerService.CreateAsync(new CustomerCreateOptions
                {
                    Email = user.Email,
                    Metadata = new Dictionary<string, string>
                    {
                        { "userId", user.Id.ToString() }
                    }
                });

                user.StripeCustomerId = stripeCustomer.Id;
                await _userRepository.UpdateAsync(user);
            }

            var options = new SessionCreateOptions
            {
                Mode = "subscription",
                Customer = user.StripeCustomerId,
                LineItems = new List<SessionLineItemOptions>
                {
                    new SessionLineItemOptions
                    {
                        Price = priceId,
                        Quantity = 1,
                    },
                },
                SuccessUrl = "http://localhost:3000/payment-success?session_id={CHECKOUT_SESSION_ID}",
                CancelUrl = "http://localhost:3000/payment-cancel",
            };

            var service = new SessionService();
            var session = await service.CreateAsync(options);

            return new CreateCheckoutSessionResponse { SessionId = session.Id };
        }

        public async Task<CreateCheckoutSessionResponse> UpgradeSubscriptionAsync(string newPriceId)
        {
            if (string.IsNullOrEmpty(_userService.UserId))
                throw new UnauthorizedAccessException("User must be authenticated.");

            var userId = long.Parse(_userService.UserId);
            var user = await _userRepository.GetByIdAsync(new UserId(userId));
            if (user == null)
                throw new NotFoundException("User not found.");

            var activeSubs = await _userSubscriptionRepository.GetByUserIdAsync(user.Id);

            var currentSub = activeSubs.FirstOrDefault(us => us.IsActive);
            if (currentSub == null)
                throw new BadRequestException("No active subscription to upgrade.");

            var stripeSubscriptionService = new SubscriptionService();
            await stripeSubscriptionService.CancelAsync(currentSub.StripeSubscriptionId, new SubscriptionCancelOptions
            {
                InvoiceNow = true,
                Prorate = true
            });

            currentSub.IsActive = false;
            currentSub.CanceledAt = DateTime.UtcNow;
            currentSub.Status = "canceled";
            await _userSubscriptionRepository.CreateOrUpdateAsync(currentSub);

            var options = new SessionCreateOptions
            {
                Mode = "subscription",
                Customer = user.StripeCustomerId,
                LineItems = new List<SessionLineItemOptions>
                {
                    new SessionLineItemOptions
                    {
                        Price = newPriceId,
                        Quantity = 1
                    }
                },
                SuccessUrl = "http://localhost:3000/payment-success?session_id={CHECKOUT_SESSION_ID}",
                CancelUrl = "http://localhost:3000/payment-cancel"
            };

            var sessionService = new SessionService();
            var session = await sessionService.CreateAsync(options);

            return new CreateCheckoutSessionResponse { SessionId = session.Id };
        }

        public async Task<List<SubscriptionPlanDto>> GetAvailableSubscriptionsAsync()
        {
            if (string.IsNullOrEmpty(_userService.UserId))
                throw new UnauthorizedAccessException("User must be authenticated to get subscriptions.");

            var productService = new ProductService();
            var priceService = new PriceService();

            var products = await productService.ListAsync(new ProductListOptions
            {
                Active = true,
                Limit = 100,
                Expand = new List<string> { "data.default_price" }
            });

            var result = new List<SubscriptionPlanDto>();

            foreach (var product in products.Data)
            {
                var prices = await priceService.ListAsync(new PriceListOptions
                {
                    Product = product.Id,
                    Active = true,
                    Limit = 1
                });

                var price = prices.Data.FirstOrDefault();
                if (price == null || price.Type != "recurring") continue;

                result.Add(new SubscriptionPlanDto
                {
                    PlanName = product.Name,
                    Description = product.Description,
                    PriceId = price.Id,
                    Price = $"{(price.UnitAmount ?? 0) / 100.0:F2} {price.Currency.ToUpper()} / {price.Recurring?.Interval}",
                    Features = product.Metadata.TryGetValue("features", out var rawFeatures)
                        ? rawFeatures.Split(',').Select(f => f.Trim()).ToList()
                        : new List<string>()
                });
            }

            return result.OrderBy(spd => spd.Price).ToList();
        }

        public async Task<List<ActiveSubscriptionDto>> GetUserSubscriptionsAsync(UserId userId)
        {
            var userSubscriptions = await _userSubscriptionRepository.GetByUserIdAsync(userId);

            var activeSubscriptions = userSubscriptions
                .Select(us => new ActiveSubscriptionDto
                {
                    SubscriptionId = us.SubscriptionId.Value,
                    Name = us.Subscription.Name,
                    Description = us.Subscription.Description,
                    Price = us.Subscription.Price,
                    Currency = us.Subscription.Currency,
                    Interval = us.Subscription.Interval,
                    StartDate = us.StartDate,
                    CurrentPeriodStart = us.CurrentPeriodStart,
                    CurrentPeriodEnd = us.CurrentPeriodEnd,
                    Status = us.Status
                })
                .ToList();

            return activeSubscriptions;
        }

        public async Task HandleStripeWebhookAsync(string json, string stripeSignatureHeader)
        {
            var secret = _stripePaymentSettings.WebhookSecret;

            Event stripeEvent;
            try
            {
                stripeEvent = EventUtility.ConstructEvent(
                    json,
                    stripeSignatureHeader,
                    secret,
                    throwOnApiVersionMismatch: false
                );
            }
            catch (StripeException ex)
            {
                _appLogger.LogError("Invalid Stripe webhook signature. Ex: {Ex}", ex);
                _appLogger.LogInformation("Stripe signature: {Signature}", stripeSignatureHeader);
                _appLogger.LogInformation("Raw body: {Body}", json);
                _appLogger.LogInformation("Secret: {Secret}", secret);
                throw new BadRequestException("Invalid Stripe webhook signature.");
            }

            UserSubscription? userSubscription;
            Invoice? invoice;
            var subscriptionService = new SubscriptionService();
            Subscription? stripeSubscription;

            switch (stripeEvent.Type)
            {
                case "checkout.session.completed":
                    var session = stripeEvent.Data.Object as Session;
                    if (session == null)
                    {
                        _appLogger.LogError("Stripe session is null.");
                        throw new BadRequestException("Stripe session is null.");
                    }

                    var customerId = session.CustomerId;
                    var subscriptionId = session.SubscriptionId;

                    if (string.IsNullOrEmpty(customerId) || string.IsNullOrEmpty(subscriptionId))
                    {
                        _appLogger.LogError("Missing data in checkout session.");
                        throw new BadRequestException("Missing data in checkout session.");
                    }

                    await ProcessSubscriptionCreatedOrUpdated(session.SubscriptionId, session.CustomerId);
                    break;

                case "customer.subscription.created":
                case "customer.subscription.updated":
                    var createdOrUpdatedSubscription = stripeEvent.Data.Object as Subscription;
                    if (createdOrUpdatedSubscription == null)
                    {
                        _appLogger.LogError("Stripe subscription is null.");
                        throw new BadRequestException("Stripe subscription is null.");
                    }

                    await ProcessSubscriptionCreatedOrUpdated(createdOrUpdatedSubscription.Id, createdOrUpdatedSubscription.CustomerId);
                    break;

                case "customer.subscription.deleted":
                    var deletedSubscription = stripeEvent.Data.Object as Subscription;
                    if (deletedSubscription == null)
                    {
                        _appLogger.LogError("Deleted subscription is null.");
                        throw new BadRequestException("Deleted subscription is null.");
                    }

                    userSubscription = await _userSubscriptionRepository.GetByStripeSubscriptionIdAsync(deletedSubscription.Id);
                    if (userSubscription == null)
                        break;

                    userSubscription.IsActive = false;
                    userSubscription.Status = deletedSubscription.Status;
                    userSubscription.CanceledAt = deletedSubscription.CanceledAt ?? DateTime.UtcNow;
                    userSubscription.CurrentPeriodEnd = (DateTime?)deletedSubscription.CurrentPeriodEnd ?? DateTime.UtcNow;

                    await _userSubscriptionRepository.CreateOrUpdateAsync(userSubscription);
                    break;

                case "invoice.payment_succeeded":
                    invoice = stripeEvent.Data.Object as Invoice;
                    if (invoice == null || string.IsNullOrEmpty(invoice.SubscriptionId))
                    {
                        _appLogger.LogError("Invalid invoice or missing subscription ID.");
                        throw new BadRequestException("Invalid invoice or missing subscription ID.");
                    }


                    stripeSubscription = await subscriptionService.GetAsync(invoice.SubscriptionId);

                    userSubscription = await _userSubscriptionRepository.GetByStripeSubscriptionIdAsync(invoice.SubscriptionId);
                    if (userSubscription == null)
                        break;

                    userSubscription.Status = stripeSubscription.Status;
                    userSubscription.IsActive = stripeSubscription.Status == "active";
                    userSubscription.CurrentPeriodStart = (DateTime?)stripeSubscription.CurrentPeriodStart ?? DateTime.UtcNow;
                    userSubscription.CurrentPeriodEnd = (DateTime?)stripeSubscription.CurrentPeriodEnd ?? DateTime.UtcNow;

                    await _userSubscriptionRepository.CreateOrUpdateAsync(userSubscription);
                    break;

                case "invoice.payment_failed":
                    invoice = stripeEvent.Data.Object as Invoice;
                    if (invoice == null || string.IsNullOrEmpty(invoice.SubscriptionId))
                    {
                        _appLogger.LogError("Invalid invoice or missing subscription ID.");
                        throw new BadRequestException("Invalid invoice or missing subscription ID.");
                    }

                    stripeSubscription = await subscriptionService.GetAsync(invoice.SubscriptionId);

                    userSubscription = await _userSubscriptionRepository.GetByStripeSubscriptionIdAsync(invoice.SubscriptionId);
                    if (userSubscription == null)
                        break;

                    userSubscription.Status = stripeSubscription.Status;
                    userSubscription.IsActive = stripeSubscription.Status == "active";
                    userSubscription.CurrentPeriodEnd = (DateTime?)stripeSubscription.CurrentPeriodEnd ?? DateTime.UtcNow;

                    await _userSubscriptionRepository.CreateOrUpdateAsync(userSubscription);
                    break;

                default:
                    break;
            }
        }

        private async Task ProcessSubscriptionCreatedOrUpdated(string subscriptionId, string customerId)
        {
            var subscriptionService = new SubscriptionService();
            var priceService = new PriceService();
            var productService = new ProductService();

            var stripeSubscription = await subscriptionService.GetAsync(subscriptionId);
            var user = await _userRepository.GetByStripeCustomerIdAsync(customerId);

            if (user == null)
                throw new NotFoundException("User not found for the given customer ID: " + customerId);

            var price = await priceService.GetAsync(stripeSubscription.Items.Data[0].Price.Id);
            var product = await productService.GetAsync(price.ProductId);

            var subscriptionInDb = await _subscriptionRepository.GetByStripePriceIdAsync(price.Id);
            if (subscriptionInDb == null)
            {
                subscriptionInDb = new Domain.Entities.Subscriptions.Subscription
                {
                    Name = product.Name,
                    Price = (double)((price.UnitAmountDecimal ?? 0) / 100m),
                    StripePriceId = price.Id,
                    StripeProductId = product.Id,
                    Currency = price.Currency.ToUpperInvariant(),
                    Interval = price.Recurring?.Interval ?? "unknown",
                    Description = product.Description ?? "",
                    IsActive = true
                };
                await _subscriptionRepository.CreateAsync(subscriptionInDb);
            }

            var userSubscription = new UserSubscription
            {
                UserId = user.Id,
                SubscriptionId = subscriptionInDb.Id,
                StripeSubscriptionId = stripeSubscription.Id,
                Status = stripeSubscription.Status,
                StartDate = (DateTime?)stripeSubscription.StartDate ?? (DateTime?)stripeSubscription.CurrentPeriodStart ?? DateTime.UtcNow,
                CurrentPeriodStart = (DateTime?)stripeSubscription.CurrentPeriodStart ?? DateTime.UtcNow,
                CurrentPeriodEnd = (DateTime?)stripeSubscription.CurrentPeriodEnd ?? DateTime.UtcNow,
                CancelAt = stripeSubscription.CancelAt,
                CanceledAt = stripeSubscription.CanceledAt,
                IsActive = stripeSubscription.Status == "active"
            };

            await _userSubscriptionRepository.CreateOrUpdateAsync(userSubscription);
        }

    }
}
