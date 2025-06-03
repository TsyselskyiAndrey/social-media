using Glowee.Application.Features.Comment.Queries;
using MediatR;

namespace Glowee.Application.Features.Comment.Commands.CreateComment
{
    public class CreateCommentCommand : IRequest<CommentDto>
    {
        public string Content { get; set; }
        public long PostId { get; set; }
        public long? ParentCommentId { get; set; }
    }
}
