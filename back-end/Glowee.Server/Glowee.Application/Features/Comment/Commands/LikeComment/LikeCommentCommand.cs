using Glowee.Domain.Entities.Comments;
using MediatR;

namespace Glowee.Application.Features.Comment.Commands.LikeComment
{
    public record LikeCommentCommand(CommentId CommentId) : IRequest<bool>;
}
