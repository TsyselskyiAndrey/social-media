using Glowee.Application.Contracts.Identity;
using Glowee.Application.Contracts.Persistence;
using Glowee.Application.Exceptions;
using Glowee.Domain.Entities.LikedPosts;
using Glowee.Domain.Entities.Users;
using MediatR;
using UnauthorizedAccessException = Glowee.Application.Exceptions.UnauthorizedAccessException;

namespace Glowee.Application.Features.Post.Commands.Likes;

/// <summary>
/// Handles the like/unlike action for a post. 
/// If the post is not liked yet, it creates a new like record; otherwise, it removes the like.
/// </summary>
public class LikeCommandHandler : IRequestHandler<LikeCommand, bool>
{
    private readonly ILikeRepository _likedPostsRepository;
    private readonly IPostRepository _postRepository;
    private readonly IUserService _userService;

    /// <summary>
    /// Initializes a new instance of the <see cref="LikeCommandHandler"/> class.
    /// </summary>
    /// <param name="likeRepository">Repository for accessing liked posts.</param>
    /// <param name="postRepository">Repository for accessing posts.</param>
    /// <param name="userService"></param>
    public LikeCommandHandler(ILikeRepository likeRepository,
     IPostRepository postRepository, IUserService userService)
    {
        _likedPostsRepository = likeRepository;
        _postRepository = postRepository;
        _userService = userService;
    }

    /// <summary>
    /// Handles the like/unlike logic.
    /// </summary>
    /// <param name="request">Command containing <c>UserId</c> and <c>PostId</c>.</param>
    /// <param name="cancellationToken">Token to cancel the operation.</param>
    /// <returns>True if a like was added, false if it was removed.</returns>
    /// <exception cref="NotFoundException">Thrown if the post or user is not found.</exception>
    public async Task<bool> Handle(LikeCommand request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(_userService.UserId))
            throw new UnauthorizedAccessException("User must be authenticated to create a post.");

        var userId = long.Parse(_userService.UserId);
        
        _postRepository.PostExists(request.PostId);
        
        var posts = await _likedPostsRepository.GetAsync();
        var postLike = posts.FirstOrDefault(x => x.PostId == request.PostId && x.UserId.Value == userId);

        if (postLike == null)
        {
            await _likedPostsRepository.CreateAsync(new LikedPost()
            {
                PostId = request.PostId,
                UserId = new UserId(userId),
            });

            return true;
        }
        await _likedPostsRepository.DeleteAsync(postLike.Id);

        return false;
    }
}
