using Glowee.Application.Contracts.Persistence;
using Glowee.Application.Exceptions;
using Glowee.Domain.Entities.LikedPosts;
using MediatR;

namespace Glowee.Application.Features.Post.Commands.Likes;

/// <summary>
/// Command for liking post, you need to send 
/// </summary>
public class LikeCommandHandler : IRequestHandler<LikeCommand, Task<bool>>
{
    private readonly ILikeRepository _likedPostsRepository;
    private readonly IPostRepository  _postRepository;
    private readonly IUserRepository _userRepository;

    public LikeCommandHandler(ILikeRepository likeRepository, 
     IPostRepository postRepository, IUserRepository userRepository)
    {
        _likedPostsRepository = likeRepository;
        _postRepository = postRepository;
        _userRepository = userRepository;
    }
    
    public async Task<Task<bool>> Handle(LikeCommand request, CancellationToken cancellationToken)
    {
        var post = _postRepository.GetByIdAsync(request.PostId);
        if (post == null)
        {
            throw new NotFoundException("Post not found");
        }
        
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
            
            return Task.FromResult(true);
        }
        await _likedPostsRepository.DeleteAsync(postLike);
        
        return Task.FromResult(false);
    }
}