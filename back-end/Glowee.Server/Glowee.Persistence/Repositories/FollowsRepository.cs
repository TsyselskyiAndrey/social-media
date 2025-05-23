using Glowee.Application.Contracts.Persistence;
using Glowee.Domain.Entities.Follows;
using Glowee.Domain.Entities.Users;
using Glowee.Persistence.DbContext;
using Microsoft.EntityFrameworkCore;

namespace Glowee.Persistence.Repositories;

public class FollowsRepository : GenericRepository<Follow, FollowId>, IFollowsRepository
{
    public FollowsRepository(SqlDbContext context) : base(context)
    {
    }

    public async Task<Follow?> UserFollow(UserId user, UserId target)
    {
       return await _context.Follows.FirstOrDefaultAsync(x => x.Follower.Id.Value == user.Value && x.Followed.Id.Value == target.Value);
    }
}