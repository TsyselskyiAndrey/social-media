using Glowee.Application.Contracts.Persistence;
using Glowee.Application.Features.Post.GeneralDto;
using Glowee.Application.Features.Post.Queries.SavedPosts;
using Glowee.Application.MappingProfiles;
using MediatR;

namespace Glowee.Application.Features.Post.Queries.Posts;

public class GetPostsQueryHandler : IRequestHandler<GetPostsQuery, IEnumerable<PostDto>>
{
    private readonly IPostRepository _postRepository;

    public GetPostsQueryHandler(IPostRepository postRepository)
    {
        _postRepository = postRepository;
    }
    
    public async Task<IEnumerable<PostDto>> Handle(GetPostsQuery request, CancellationToken cancellationToken)
    {
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
            .Select(x => x.MapPostToPostDto());
        
        return posts;
    }
}