using Glowee.Domain.Entities.Subscriptions;

namespace Glowee.Application.Contracts.Persistence
{
    public interface ISubscriptionRepository : IGenericRepository<Subscription, SubscriptionId>
    {
        Task<Subscription?> GetByStripePriceIdAsync(string priceId);
    }
}
