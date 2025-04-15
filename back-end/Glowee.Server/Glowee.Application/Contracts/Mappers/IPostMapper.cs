using Glowee.Application.Features.Post.GeneralDto;
using Glowee.Domain.Entities.Posts;
using Glowee.Domain.Entities.Users;

namespace Glowee.Application.Contracts.Mappers
{
    public interface IPostMapper
    {
        PostDto MapPostToPostDtoAsync(Post post, UserId? currentUserId);
    }
}
