using Glowee.Application.Contracts.Identity;
using Glowee.Application.Contracts.Mappers;
using Glowee.Application.Contracts.Persistence;
using Glowee.Application.Features.Post.GeneralDto;
using Glowee.Domain.Entities.Users;
using UnauthorizedAccessException = System.UnauthorizedAccessException;

namespace Glowee.Application.Features.Post.Queries.Posts;

public class GetSavedPostsQueryHandler
{
    private readonly IPostRepository _postRepository;
    private readonly IPostMapper _postMapper;
    private readonly IUserService _userService;

    public GetSavedPostsQueryHandler(IPostRepository postRepository, IPostMapper postMapper, IUserService userService)
    {
        _postRepository = postRepository;
        _postMapper = postMapper;
        _userService = userService;
    }

    public async Task<IEnumerable<PostDto>> Handle(GetSavedPostsQuery request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(_userService.UserId))
            throw new UnauthorizedAccessException("User must be authenticated to posts.");

        var userId = long.Parse(_userService.UserId);
        var posts = await _postRepository.GetUsersSavedPostsAsync(new UserId(userId));
        var result = posts.Select(p => _postMapper.MapPostToPostDto(p, new UserId(userId)));

        return result;
    }
}