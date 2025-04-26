using MediatR;

namespace Glowee.Application.Features.Post.Commands.Comment.CreateComment
{
    public class CreateCommentCommand : IRequest
    {
        public string Content { get; set; }
        public long PostId { get; set; }
        public long? ParentCommentId { get; set; }
    } 
}
