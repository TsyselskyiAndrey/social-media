using Glowee.Application.Models.StripePayment;
using Glowee.Domain.Entities.Users;

namespace Glowee.Application.Contracts.StripePayment
{
    public interface IStripePaymentService
    {
        string Config();
        Task<CreateCheckoutSessionResponse> CreateCheckoutSessionAsync(string priceId);
        Task<CreateCheckoutSessionResponse> UpgradeSubscriptionAsync(string newPriceId);
        Task CancelSubscriptionAsync(string priceId);
        Task<List<SubscriptionPlanDto>> GetAvailableSubscriptionsAsync();
        Task<List<ActiveSubscriptionDto>> GetUserSubscriptionsAsync(UserId userId);
        Task HandleStripeWebhookAsync(string json, string stripeSignatureHeader);
    }
}
