using Glowee.Domain.Entities.Posts;

namespace Glowee.Application.Contracts.Persistence;

public interface IPostRepository: IGenericRepository<Post, PostId>
{
    
}