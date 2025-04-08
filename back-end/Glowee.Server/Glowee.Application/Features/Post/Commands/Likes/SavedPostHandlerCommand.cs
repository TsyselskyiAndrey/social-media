using Glowee.Application.Contracts.Persistence;
using Glowee.Application.Exceptions;
using Glowee.Domain.Entities.SavedPosts;
using MediatR;

namespace Glowee.Application.Features.Post.Commands.Likes;

/// <summary>
/// Handles the save/unsaved action for a post. 
/// If the post is not saved yet, it creates a new saved post record; otherwise, it removes the existed saved post.
/// </summary>
public class SavedPostHandlerCommand : IRequestHandler<SavedPostCommand, bool>
{
    private readonly ISavedPostRepository _savedPostRepository;
    private readonly IUserRepository _userRepository;
    private readonly IPostRepository _postRepository;

    /// <summary>
    /// Initializes a new instance of the <see cref="SavedPostHandlerCommand"/> class.
    /// </summary>
    /// <param name="savedPostRepository">Repository for accessing saved posts.</param>
    /// <param name="postRepository">Repository for accessing posts.</param>
    /// <param name="userRepository">Repository for accessing users.</param>
    public SavedPostHandlerCommand(IPostRepository postRepository
        , IUserRepository userRepository, ISavedPostRepository savedPostRepository)
    {
        _savedPostRepository = savedPostRepository;
        _postRepository = postRepository;
        _userRepository = userRepository;
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
        var post = await _postRepository.GetByIdAsync(request.PostId);
        if (post == null)
        {
            throw new NotFoundException("Post not found");
        }
        
        var user = await _userRepository.GetByIdAsync(request.UserId);
        if (user == null)
        {
            throw new NotFoundException("User not found");
        }

        var posts = await _savedPostRepository.GetAsync();
        var postLike = posts.FirstOrDefault(x => x.PostId == request.PostId && x.UserId == request.UserId);

        if (postLike == null)
        {
            await _savedPostRepository.CreateAsync(new SavedPost()
            {
                PostId = request.PostId,
                UserId = request.UserId,
            });
            
            return true;
        }
        await _savedPostRepository.DeleteAsync(postLike);
        
        return false;
    }
}