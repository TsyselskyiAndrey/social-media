using MediatR;

namespace Glowee.Application.Features.Post.Commands.Comment.EditComment;

public class EditCommentCommand : IRequest
{
    public long CommentId { get; set; }
    public string Content { get; set; }
}