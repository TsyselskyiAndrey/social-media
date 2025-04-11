using Glowee.Application.Features.Post.Queries.SavedPosts;
using Glowee.Domain.Entities.Posts;

namespace Glowee.Application.Contracts.Persistence;

public interface IPostRepository: IGenericRepository<Post, PostId>
{
    void PostExists(PostId id);
    
    Task<IEnumerable<Post>> GetIncludedPosts();
}