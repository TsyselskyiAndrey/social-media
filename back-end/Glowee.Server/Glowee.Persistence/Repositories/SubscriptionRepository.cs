using Glowee.Application.Contracts.Persistence;
using Glowee.Domain.Entities.Subscriptions;
using Glowee.Persistence.DbContext;
using Microsoft.EntityFrameworkCore;

namespace Glowee.Persistence.Repositories
{
    public class SubscriptionRepository : GenericRepository<Subscription, SubscriptionId>, ISubscriptionRepository
    {
        public SubscriptionRepository(SqlDbContext context) : base(context)
        {
        }

        public async Task<Subscription?> GetByStripePriceIdAsync(string priceId)
        {
            return await _context.Subscriptions.FirstOrDefaultAsync(x => x.StripePriceId == priceId);
        }
    }
}
