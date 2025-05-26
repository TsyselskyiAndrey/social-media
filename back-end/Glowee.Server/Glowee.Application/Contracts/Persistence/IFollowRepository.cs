using Glowee.Domain.Entities.Follows;
using Glowee.Domain.Entities.Users;

namespace Glowee.Application.Contracts.Persistence
{
    public interface IFollowRepository : IGenericRepository<Follow, FollowId>
    {
        Task DeleteByFollowerIdAsync(UserId userId);
        Task<Follow?> UserFollow(UserId user, UserId target);
    }
}
