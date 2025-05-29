using Glowee.Domain.Entities.Users;
using Glowee.Domain.Entities.UserSubscriptions;

namespace Glowee.Application.Contracts.Persistence
{
    public interface IUserSubscriptionRepository : IGenericRepository<UserSubscription, UserSubscriptionId>
    {
        Task CreateOrUpdateAsync(UserSubscription userSubscription);
        Task<UserSubscription?> GetByStripeSubscriptionIdAsync(string stripeSubscriptionId);
        Task<IEnumerable<UserSubscription>> GetByUserIdAsync(UserId userId);
    }
}
