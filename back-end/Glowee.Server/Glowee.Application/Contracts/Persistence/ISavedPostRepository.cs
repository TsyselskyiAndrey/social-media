using Glowee.Domain.Entities.SavedPosts;

namespace Glowee.Application.Contracts.Persistence;

public interface ISavedPostRepository: IGenericRepository<SavedPost, SavedPostId>
{
    
}