using System.Text.RegularExpressions;
using Glowee.Api.Requests;
using Glowee.Application.Contracts.Identity;
using Glowee.Application.Contracts.Logging;
using Glowee.Application.Contracts.Mappers;
using Glowee.Application.Contracts.Persistence;
using Glowee.Application.Contracts.Storage;
using Glowee.Application.Features.Post.Commands.Likes;
using Glowee.Application.Features.Post.Commands.Post.CreatePost;
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


    public PostController(IPostRepository postRepository, IPostMapper postMapper, IUserService userService,
    ITagRepository tagRepository, IPostMediaRepository postMediaRepository, IPostMediaStorageService postMediaStorageService,
    ILikeRepository likeRepository, ISavedPostRepository savedPostRepository, IAppLogger<PostController> logger)
    {
        _postRepository = postRepository;
        _userService = userService;
        _postMapper = postMapper;
        _tagRepository = tagRepository;
        _postMediaRepository = postMediaRepository;
        _postMediaStorageService = postMediaStorageService;
        _likeRepository = likeRepository;
        _savedPostRepository = savedPostRepository;
        _logger = logger;
    }

    [HttpGet("getPosts")]
    public async Task<IActionResult> GetPostsAsync([FromQuery] int postAmount, 
        [FromQuery] long postId, [FromQuery] List<long> tags, [FromQuery] long userId)
    {
        var request = new GetPostsQuery()
        {
            PostsAmount = postAmount,
            LastPostId = new PostId(postId),
            Tags = tags.Select(t => new TagId(t)).ToList(),
            UserId = new UserId(userId),
        };

        var handler = new GetPostsQueryHandler(_postRepository, _postMapper, _userService);
        var posts = await handler.Handle(request, new CancellationToken());

        return Ok(posts);
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