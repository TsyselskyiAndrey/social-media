using Glowee.Application.Models.StripePayment;

namespace Glowee.Application.Contracts.StripePayment
{
    public interface IStripePaymentService
    {
        string Config();
        Task<CreateCheckoutSessionResponse> CreateCheckoutSessionAsync(string priceId);
        Task<List<SubscriptionPlanDto>> GetAvailableSubscriptionsAsync();
    }
}
