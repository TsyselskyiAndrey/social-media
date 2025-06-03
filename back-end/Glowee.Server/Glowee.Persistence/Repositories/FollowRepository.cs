using Glowee.Application.Contracts.Persistence;
using Glowee.Domain.Entities.Follows;
using Glowee.Domain.Entities.Users;
using Glowee.Persistence.DbContext;
using Microsoft.EntityFrameworkCore;

namespace Glowee.Persistence.Repositories
{
    public class FollowRepository : GenericRepository<Follow, FollowId>, IFollowRepository
    {
        public FollowRepository(SqlDbContext context) : base(context)
        {
        }

        public async Task DeleteByFollowerIdAsync(UserId userId)
        {
            await _context.Follows
                .Where(f => f.FollowerId == userId)
                .ExecuteDeleteAsync();
        }

        public async Task<Follow?> UserFollow(UserId user, UserId target)
        {
            return await _context.Follows.FirstOrDefaultAsync(x => x.FollowerId == user && x.FollowedId == target);
        }
    }
}
