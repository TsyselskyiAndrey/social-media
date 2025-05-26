using MediatR;

namespace Glowee.Application.Features.Comment.Commands.EditComment;

public class EditCommentCommand : IRequest
{
    public long CommentId { get; set; }
    public string Content { get; set; }
}