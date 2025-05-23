using Glowee.Application.Features.Post.Queries.SavedPosts;
using Glowee.Domain.Entities.Posts;
using Glowee.Domain.Entities.SavedPosts;
using Glowee.Domain.Entities.Users;

namespace Glowee.Application.Contracts.Persistence;

public interface ISavedPostRepository: IGenericRepository<SavedPost, SavedPostId>
{
    public Task<IEnumerable<Post>> GetUserSavedPostsAsync(UserId userId);
}