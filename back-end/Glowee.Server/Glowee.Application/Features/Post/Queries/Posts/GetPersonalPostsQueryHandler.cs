using Glowee.Application.Contracts.Identity;
using Glowee.Application.Contracts.Persistence;
using Glowee.Application.Contracts.Storage;
using Glowee.Application.Features.Post.GeneralDto;
using Glowee.Application.MappingProfiles;
using Glowee.Domain.Entities.Users;
using MediatR;

namespace Glowee.Application.Features.Post.Queries.Posts;

public class GetPersonalPostsQueryHandler : IRequestHandler<GetPersonalPostsQuery ,IEnumerable<PostDto>>
{
    private readonly IUserService _userService;
    private readonly IPostRepository _postRepository;
    private readonly IProfileImageStorageService _profileImageStorageService;
    private readonly IThumbnailStorageService _thumbnailStorageService;
    private readonly IPostMediaStorageService _postMediaStorageService;

    public GetPersonalPostsQueryHandler(IUserService userService, IPostRepository postRepository
        , IThumbnailStorageService thumbnailStorageService, IPostMediaStorageService postMediaStorageService
        , IProfileImageStorageService profileImageStorageService)
    {
        _userService = userService;
        _postRepository = postRepository;
        _thumbnailStorageService = thumbnailStorageService;
        _postMediaStorageService = postMediaStorageService;
        _profileImageStorageService = profileImageStorageService;
    }
    
    public async Task<IEnumerable<PostDto>> Handle(GetPersonalPostsQuery request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(_userService.UserId))
            throw new UnauthorizedAccessException("User must be authenticated to posts.");

        var userId = long.Parse(_userService.UserId);
        var userStrongId = new UserId(userId);
        
        var usersPosts = await _postRepository.GetUsersPersonalPostsAsync(userStrongId);

        var postMapper = new PostMapper(_thumbnailStorageService, _postMediaStorageService, _profileImageStorageService);
        
        //var mappedPosts = usersPosts.Select(x => postMapper.MapPostToPostDto(x, userStrongId)).ToList();
        
        var mappedPosts = new List<PostDto>();

        foreach (var post in usersPosts)
        {
            mappedPosts.Add(postMapper.MapPostToPostDto(post, userStrongId));
        }
        
        return mappedPosts;
    }
}