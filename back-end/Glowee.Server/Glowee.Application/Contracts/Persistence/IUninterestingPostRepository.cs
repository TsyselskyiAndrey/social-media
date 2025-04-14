using Glowee.Domain.Entities.UninterestingPosts;

namespace Glowee.Application.Contracts.Persistence;

public interface IUninterestingPostRepository : IGenericRepository<UninterestingPost, UninterestingPostId>
{
    
}