using Glowee.Application.Features.Post.Queries.Comments;
using Glowee.Domain.Entities.Comments;
using Glowee.Domain.Entities.Users;

namespace Glowee.Application.Contracts.Mappers
{
    public interface ICommentMapper
    {
        CommentDto MapCommentToCommentDto(Comment comment, UserId? currentUserId = null);
    }
}
