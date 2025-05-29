using Glowee.Application.Contracts.Identity;
using Glowee.Application.Contracts.Mappers;
using Glowee.Application.Contracts.Persistence;
using Glowee.Application.Exceptions;
using Glowee.Application.Features.Post.GeneralDto;
using Glowee.Domain.Entities.Users;
using MediatR;
using UnauthorizedAccessException = System.UnauthorizedAccessException;

namespace Glowee.Application.Features.Post.Queries.Posts;

public class GetPostsQueryHandler : IRequestHandler<GetPostsQuery, IEnumerable<PostDto>>
{
    private readonly IPostRepository _postRepository;
    private readonly IPostMapper _postMapper;
    private readonly IUserService _userService;
    private readonly IUserRepository _userRepository;

    public GetPostsQueryHandler(IPostRepository postRepository, IPostMapper postMapper, IUserService userService, IUserRepository userRepository)
    {
        _postRepository = postRepository;
        _postMapper = postMapper;
        _userService = userService;
        _userRepository = userRepository;
    }

    public async Task<IEnumerable<PostDto>> Handle(GetPostsQuery request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(_userService.UserId))
            throw new UnauthorizedAccessException("User must be authenticated to posts.");

        if (request.UserId != null)
        {
            var user = await _userRepository.GetByIdAsync(new UserId(request.UserId.Value));

            if (user == null)
                throw new BadRequestException("User was not found.");
        }
        
        var userId = long.Parse(_userService.UserId);
        var posts = await _postRepository.GetFilteredPostsAsync(request.PostTitle, request.PostsAmount
            , request.LastPostId, request.Tags, request.UserId, userId);
        
        var result = posts.Select(p => _postMapper.MapPostToPostDto(p, new UserId(userId)));
        
        return result;
    }
}