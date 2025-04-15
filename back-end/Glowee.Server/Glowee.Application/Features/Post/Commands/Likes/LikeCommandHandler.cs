using Glowee.Application.Contracts.Persistence;
using Glowee.Application.Exceptions;
using Glowee.Domain.Entities.LikedPosts;
using MediatR;

namespace Glowee.Application.Features.Post.Commands.Likes;

/// <summary>
/// Handles the like/unlike action for a post. 
/// If the post is not liked yet, it creates a new like record; otherwise, it removes the like.
/// </summary>
public class LikeCommandHandler : IRequestHandler<LikeCommand, bool>
{
    private readonly ILikeRepository _likedPostsRepository;
    private readonly IPostRepository _postRepository;
    private readonly IUserRepository _userRepository;

    /// <summary>
    /// Initializes a new instance of the <see cref="LikeCommandHandler"/> class.
    /// </summary>
    /// <param name="likeRepository">Repository for accessing liked posts.</param>
    /// <param name="postRepository">Repository for accessing posts.</param>
    /// <param name="userRepository">Repository for accessing users.</param>
    public LikeCommandHandler(ILikeRepository likeRepository,
     IPostRepository postRepository, IUserRepository userRepository)
    {
        _likedPostsRepository = likeRepository;
        _postRepository = postRepository;
        _userRepository = userRepository;
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
        _postRepository.PostExists(request.PostId);

        var user = await _userRepository.GetByIdAsync(request.UserId);
        if (user == null)
        {
            throw new NotFoundException("User not found");
        }

        var posts = await _likedPostsRepository.GetAsync();
        var postLike = posts.FirstOrDefault(x => x.PostId == request.PostId && x.UserId == request.UserId);

        if (postLike == null)
        {
            await _likedPostsRepository.CreateAsync(new LikedPost()
            {
                PostId = request.PostId,
                UserId = request.UserId,
            });

            return true;
        }
        await _likedPostsRepository.DeleteAsync(postLike.Id);

        return false;
    }
}
