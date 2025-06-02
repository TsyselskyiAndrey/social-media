using Glowee.Application.Contracts.Identity;
using Glowee.Application.Contracts.Persistence;
using Glowee.Application.Contracts.Storage;
using Glowee.Application.Helpers;
using Glowee.Domain.Entities.Posts;
using Glowee.Domain.Entities.Users;
using MediatR;

namespace Glowee.Application.Features.Post.Commands.Post.DeletePost;

public class DeletePostCommandHandler : IRequestHandler<DeletePostCommand>
{
    private readonly IPostRepository _postRepository;
    private readonly IUserService _userService;

    public DeletePostCommandHandler(IPostRepository postRepository, IUserService userService)
    {
        _postRepository = postRepository;
        _userService = userService;
    }

    public async Task Handle(DeletePostCommand request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(_userService.UserId))
            throw new UnauthorizedAccessException("User must be authenticated to delete a post.");
        
        var userId = long.Parse(_userService.UserId);
        
        await _postRepository.DeleteWithDependenciesAsync(request.PostId, new UserId(userId));
    }
}
