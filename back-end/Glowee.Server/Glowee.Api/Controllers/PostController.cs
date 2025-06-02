using Glowee.Api.Requests.Post;
using Glowee.Application.Contracts.Identity;
using Glowee.Application.Contracts.Logging;
using Glowee.Application.Contracts.Mappers;
using Glowee.Application.Contracts.Persistence;
using Glowee.Application.Contracts.Storage;
using Glowee.Application.Features.Post.Commands.Likes;
using Glowee.Application.Features.Post.Commands.Post.CreatePost;
using Glowee.Application.Features.Post.Commands.Post.DeletePost;
using Glowee.Application.Features.Post.Commands.Post.UpdatePost;
using Glowee.Application.Features.Post.Commands.SavedPosts;
using Glowee.Application.Features.Post.Queries.Posts;
using Glowee.Domain.Entities.Posts;
using Glowee.Domain.Entities.Tags;
using Glowee.Domain.Entities.Users;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Glowee.Api.Controllers;

[ApiController]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
[Route("api/[controller]")]
public class PostController : ControllerBase
{
    private readonly IPostRepository _postRepository;
    private readonly IPostMapper _postMapper;
    private readonly IUserService _userService;
    private readonly IPostMediaStorageService _postMediaStorageService;
    private readonly IPostMediaRepository _postMediaRepository;
    private readonly ITagRepository _tagRepository;
    private readonly ILikeRepository _likeRepository;
    private readonly ISavedPostRepository _savedPostRepository;
    private readonly IAppLogger<PostController> _logger;
    private readonly IUserRepository _userRepository;
    
    public PostController(IPostRepository postRepository, IPostMapper postMapper, IUserService userService,
    ITagRepository tagRepository, IPostMediaRepository postMediaRepository, IPostMediaStorageService postMediaStorageService,
    ILikeRepository likeRepository, ISavedPostRepository savedPostRepository, IUserRepository userRepository,IAppLogger<PostController> logger)
    {
        _postRepository = postRepository;
        _userService = userService;
        _postMapper = postMapper;
        _tagRepository = tagRepository;
        _postMediaRepository = postMediaRepository;
        _postMediaStorageService = postMediaStorageService;
        _likeRepository = likeRepository;
        _savedPostRepository = savedPostRepository;
        _userRepository = userRepository;
        _logger = logger;
    }

    [HttpGet("getPosts")]
    public async Task<IActionResult> GetPostsAsync([FromQuery] string? postTitle,[FromQuery] int postAmount,
        [FromQuery] long? postId, [FromQuery] List<long>? tags, [FromQuery] long? userId)
    {
        List<TagId> list = new List<TagId>();
        if (tags != null)
            foreach (var t in tags) list.Add(new TagId(t));

        var request = new GetPostsQuery()
        {
            PostTitle = postTitle,
            PostsAmount = postAmount,
            LastPostId = postId,
            Tags = list,
            UserId = userId,
        };

        var handler = new GetPostsQueryHandler(_postRepository, _postMapper, _userService, _userRepository);
        var posts = await handler.Handle(request, new CancellationToken());

        return Ok(posts);
    }

    [HttpGet("getSavedPosts")]
    public async Task<IActionResult> GetUsersSavedPostsAsync()
    {
        var request = new GetSavedPostsQuery();
        var handler = new GetSavedPostsQueryHandler(_postRepository, _postMapper, _userService);
        
        var result = await handler.Handle(request, CancellationToken.None);
        return Ok(result);
    }
    
    [HttpPost("createPost")]
    public async Task<IActionResult> CreatePostAsync([FromForm] CreatePostRequest postDto)
    {
        var command = new CreatePostCommand()
        {
            Caption = postDto.Caption,
            Tags = postDto.Tags,
            PostMedias = postDto.PostMedias,
            Thumbnail = postDto.Thumbnail,
        };

        var handler = new CreatePostCommandHandler(_userService, _postRepository, _tagRepository);

        await handler.Handle(command, new CancellationToken());
        return Ok();
    }

    [HttpPut("updatePost")]
    public async Task<IActionResult> UpdatePost([FromForm] UpdatePostRequest request)
    {
        var command = new EditPostCommand()
        {
            Id = request.Id,
            Caption = request.Caption,
            Tags = request.Tags,
            PostMedias = request.PostMedias,
            Thumbnail = request.Thumbnail,
        };

        var handler = new EditPostCommandHandler(_postRepository, _postMediaStorageService, _tagRepository, _userService);
        
        await handler.Handle(command, new CancellationToken());
        return Ok();
    }
    
    [HttpDelete("deletePost/{postId}")]
    public async Task<IActionResult> UpdatePost(long postId)
    {
        var command = new DeletePostCommand(new PostId(postId));

        var handler = new DeletePostCommandHandler(_postRepository, _userService);
        
        await handler.Handle(command, new CancellationToken());
        return Ok();
    }
    
    [HttpPost("like")]
    public async Task<IActionResult> LikePostAsync([FromBody] LikePostRequest likeRequest)
    {
        var command = new LikeCommand(new PostId(likeRequest.PostId));

        var handler = new LikeCommandHandler(_likeRepository, _postRepository, _userService);

        var result = await handler.Handle(command, new CancellationToken());
        return Ok(result);
    }

    [HttpPost("save")]
    public async Task<IActionResult> SavePostAsync([FromBody] SavePostRequest saveRequest)
    {
        var command = new SavedPostCommand(new PostId(saveRequest.PostId));

        var handler = new SavedPostCommandHandler(_postRepository, _savedPostRepository, _userService);

        var result = await handler.Handle(command, new CancellationToken());
        return Ok(result);
    }

    [HttpGet("getAllTags")]
    public async Task<IActionResult> GetAllTags()
    {
        var command = new GetAllTagsQuery();

        var handler = new GetAllTagsQueryHandler(_tagRepository, _userService);

        var tags = await handler.Handle(command, new CancellationToken());
        return Ok(tags);
    }


}