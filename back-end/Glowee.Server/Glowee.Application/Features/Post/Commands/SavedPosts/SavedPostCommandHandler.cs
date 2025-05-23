using Glowee.Application.Contracts.Identity;
using Glowee.Application.Contracts.Persistence;
using Glowee.Application.Exceptions;
using Glowee.Domain.Entities.SavedPosts;
using Glowee.Domain.Entities.Users;
using MediatR;
using UnauthorizedAccessException = Glowee.Application.Exceptions.UnauthorizedAccessException;

namespace Glowee.Application.Features.Post.Commands.SavedPosts;

/// <summary>
/// Handles the save/unsaved action for a post. 
/// If the post is not saved yet, it creates a new saved post record; otherwise, it removes the existed saved post.
/// </summary>
public class SavedPostCommandHandler : IRequestHandler<SavedPostCommand, bool>
{
    private readonly ISavedPostRepository _savedPostRepository;
    private readonly IPostRepository _postRepository;
    private readonly IUserService _userService;

    /// <summary>
    /// Initializes a new instance of the <see cref="SavedPostHandlerCommand"/> class.
    /// </summary>
    /// <param name="savedPostRepository">Repository for accessing saved posts.</param>
    /// <param name="postRepository">Repository for accessing posts.</param>
    /// <param name="userRepository">Repository for accessing users.</param>
    public SavedPostCommandHandler(IPostRepository postRepository,
        ISavedPostRepository savedPostRepository, IUserService userService)
    {
        _savedPostRepository = savedPostRepository;
        _postRepository = postRepository;
        _userService = userService;
    }

    /// <summary>
    /// Handles the save/unsaved post logic.
    /// </summary>
    /// <param name="request">Command containing <c>UserId</c> and <c>PostId</c>.</param>
    /// <param name="cancellationToken">Token to cancel the operation.</param>
    /// <returns>True if a like was added, false if it was removed.</returns>
    /// <exception cref="NotFoundException">Thrown if the post or user is not found.</exception>
    public async Task<bool> Handle(SavedPostCommand request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(_userService.UserId))
            throw new UnauthorizedAccessException("User must be authenticated to create a post.");

        var userId = long.Parse(_userService.UserId);
        
        _postRepository.PostExists(request.PostId);

        var posts = await _savedPostRepository.GetAsync();
        var postLike = posts.FirstOrDefault(x => x.PostId == request.PostId && x.UserId.Value == userId);

        if (postLike == null)
        {
            await _savedPostRepository.CreateAsync(new SavedPost()
            {
                PostId = request.PostId,
                UserId = new UserId(userId),
            });

            return true;
        }
        await _savedPostRepository.DeleteAsync(postLike.Id);

        return false;
    }
}