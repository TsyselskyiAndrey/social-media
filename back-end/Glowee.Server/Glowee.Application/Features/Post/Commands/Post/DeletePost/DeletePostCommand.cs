using MediatR;

namespace Glowee.Application.Features.Post.Commands.Post.DeletePost;

public class DeletePostCommand : IRequest
{
    public long PostId { get; set; }
}