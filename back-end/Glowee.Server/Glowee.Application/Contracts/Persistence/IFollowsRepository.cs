using Glowee.Domain.Entities.Follows;
using Glowee.Domain.Entities.Users;

namespace Glowee.Application.Contracts.Persistence;

public interface IFollowsRepository : IGenericRepository<Follow, FollowId>
{
    Task<Follow?> UserFollow(UserId user, UserId target);
}