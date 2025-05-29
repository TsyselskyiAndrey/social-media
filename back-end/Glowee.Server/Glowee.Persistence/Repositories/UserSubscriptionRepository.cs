using Glowee.Application.Contracts.Logging;
using Glowee.Application.Contracts.Persistence;
using Glowee.Domain.Entities.Users;
using Glowee.Domain.Entities.UserSubscriptions;
using Glowee.Persistence.DbContext;
using Microsoft.EntityFrameworkCore;

namespace Glowee.Persistence.Repositories
{
    public class UserSubscriptionRepository : GenericRepository<UserSubscription, UserSubscriptionId>, IUserSubscriptionRepository
    {
        private readonly IAppLogger<UserSubscriptionRepository> _appLogger;
        public UserSubscriptionRepository(SqlDbContext context, IAppLogger<UserSubscriptionRepository> appLogger) : base(context)
        {
            _appLogger = appLogger;
        }

        public async Task CreateOrUpdateAsync(UserSubscription userSubscription)
        {
            var existing = await _context.UserSubscriptions.FirstOrDefaultAsync(us =>
                            us.UserId == userSubscription.UserId &&
                            us.SubscriptionId == userSubscription.SubscriptionId);


            if (existing == null)
            {
                await CreateAsync(userSubscription);
            }
            else
            {
                existing.StripeSubscriptionId = userSubscription.StripeSubscriptionId;
                existing.Status = userSubscription.Status;
                existing.StartDate = userSubscription.StartDate;
                existing.CurrentPeriodStart = userSubscription.CurrentPeriodStart;
                existing.CurrentPeriodEnd = userSubscription.CurrentPeriodEnd;
                existing.CancelAt = userSubscription.CancelAt;
                existing.CanceledAt = userSubscription.CanceledAt;
                existing.IsActive = userSubscription.IsActive;

                await UpdateAsync(existing);
            }
        }

        public async Task<UserSubscription?> GetByStripeSubscriptionIdAsync(string stripeSubscriptionId)
        {
            return await _context.UserSubscriptions.FirstOrDefaultAsync(x => x.StripeSubscriptionId == stripeSubscriptionId);
        }

        public async Task<IEnumerable<UserSubscription>> GetByUserIdAsync(UserId userId)
        {
            return await _context.UserSubscriptions.Where(x => x.UserId == userId).Include(x => x.Subscription).ToListAsync();
        }
    }
}
