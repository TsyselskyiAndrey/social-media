using Glowee.Application.Contracts.Identity;
using Glowee.Application.Contracts.Persistence;
using Glowee.Application.Contracts.StripePayment;
using Glowee.Application.Exceptions;
using Glowee.Application.Models.StripePayment;
using Glowee.Domain.Entities.Users;
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

        public StripePaymentService(IOptions<StripePaymentSettings> stripePaymentSettings, IUserService userService, IUserRepository userRepository)
        {
            _stripePaymentSettings = stripePaymentSettings.Value;
            _userService = userService;
            _userRepository = userRepository;
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

            var options = new SessionCreateOptions
            {
                PaymentMethodTypes = new List<string> { "card" },
                Mode = "subscription",
                CustomerEmail = user.Email,
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

        public async Task HandleStripeWebhookAsync(string json, string stripeSignatureHeader)
        {
            var secret = _stripePaymentSettings.WebhookSecret;

            Event stripeEvent;
            try
            {
                stripeEvent = EventUtility.ConstructEvent(
                    json,
                    stripeSignatureHeader,
                    secret
                );
            }
            catch (StripeException ex)
            {
                throw new BadRequestException("Invalid Stripe webhook signature.");
            }

            switch (stripeEvent.Type)
            {
                case "checkout.session.completed":
                    var session = stripeEvent.Data.Object as Session;
                    // TODO: обработка
                    break;

                case "customer.subscription.created":
                    break;
                case "customer.subscription.updated":
                    break;
                case "customer.subscription.deleted":
                    var subscription = stripeEvent.Data.Object as Subscription;
                    // TODO: обновление
                    break;

                case "invoice.payment_succeeded":
                    // TODO: логика
                    break;

                case "invoice.payment_failed":
                    // TODO: логика
                    break;

                default:
                    // Неизвестное событие
                    break;
            }
        }

    }
}
