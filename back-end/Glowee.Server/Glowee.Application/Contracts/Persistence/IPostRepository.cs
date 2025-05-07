using Glowee.Domain.Entities.Posts;
using Glowee.Domain.Entities.Users;

namespace Glowee.Application.Contracts.Persistence;

public interface IPostRepository : IGenericRepository<Post, PostId>
{
    Task DeleteByUserIdAsync(UserId userId);

    void PostExists(PostId id);

    Task<IEnumerable<Post>> GetIncludedPosts();
}