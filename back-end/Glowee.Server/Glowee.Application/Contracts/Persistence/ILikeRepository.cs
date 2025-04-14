using Glowee.Domain.Entities.LikedPosts;

namespace Glowee.Application.Contracts.Persistence;

public interface ILikeRepository : IGenericRepository<LikedPost, LikedPostId>
{
        
}