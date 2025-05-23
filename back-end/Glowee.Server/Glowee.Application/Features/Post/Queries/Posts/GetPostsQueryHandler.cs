using Glowee.Application.Contracts.Identity;
using Glowee.Application.Contracts.Mappers;
using Glowee.Application.Contracts.Persistence;
using Glowee.Application.Features.Post.GeneralDto;
using MediatR;

namespace Glowee.Application.Features.Post.Queries.Posts;

public class GetPostsQueryHandler : IRequestHandler<GetPostsQuery, IEnumerable<PostDto>>
{
    private readonly IPostRepository _postRepository;
    private readonly IPostMapper _postMapper;
    private readonly IUserService _userService;

    public GetPostsQueryHandler(IPostRepository postRepository, IPostMapper postMapper, IUserService userService)
    {
        _postRepository = postRepository;
        _postMapper = postMapper;
        _userService = userService;
    }

    public async Task<IEnumerable<PostDto>> Handle(GetPostsQuery request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(_userService.UserId))
            throw new UnauthorizedAccessException("User must be authenticated to posts.");
        
        var postsQuery = await _postRepository
            .GetIncludedPosts();

        if (request.Tags is { Count: > 0 })
        {
            postsQuery = postsQuery.Where(p => p.Tags.Any(tag => request.Tags.Contains(tag.Id)));
        }

        if (request.UserId is not null)
        {
            postsQuery = postsQuery.Where(p => p.UninterestingPosts.All(up => up.UserId != request.UserId));
        }

        if (request.LastPostId is not null)
        {
            var lastPost = await _postRepository.GetByIdAsync(request.LastPostId);
            if (lastPost != null)
            {
                postsQuery = postsQuery.Where(p => p.Id.Value < lastPost.Id.Value);
            }
        }
        
        var posts = postsQuery
            .OrderByDescending(p => p.Id.Value)
            .Take(request.PostsAmount)
            .Select(x => _postMapper.MapPostToPostDtoAsync(x, request.UserId));

        return posts;
    }
}