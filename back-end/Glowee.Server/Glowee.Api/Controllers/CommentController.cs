using Glowee.Api.Requests.Comment;
using Glowee.Application.Contracts.Identity;
using Glowee.Application.Contracts.Mappers;
using Glowee.Application.Contracts.Persistence;
using Glowee.Application.Features.Comment.Commands.CreateComment;
using Glowee.Application.Features.Comment.Commands.DeleteComment;
using Glowee.Application.Features.Comment.Commands.EditComment;
using Glowee.Application.Features.Comment.Commands.LikeComment;
using Glowee.Application.Features.Comment.Queries;
using Glowee.Domain.Entities.Comments;
using Glowee.Domain.Entities.Posts;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Glowee.Api.Controllers
{
    [Route("api/[controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    public class CommentController : ControllerBase
    {
        private readonly ICommentRepository _commentRepository;
        private readonly IPostRepository _postRepository;
        private readonly ICommentMapper _commentMapper;
        private readonly IUserService _userService;
        private readonly ICommentStatusRepository _commentStatusRepository;

        public CommentController(ICommentRepository commentRepository, IPostRepository postRepository, ICommentMapper commentMapper, IUserService userService, ICommentStatusRepository commentStatusRepository)
        {
            _commentRepository = commentRepository;
            _postRepository = postRepository;
            _commentMapper = commentMapper;
            _userService = userService;
            _commentStatusRepository = commentStatusRepository;
        }

        [HttpGet("getComments")]
        public async Task<IActionResult> GetCommentsAsync([FromQuery] long postId)
        {
            var request = new GetPostCommentsQuery(new PostId(postId));

            var handler = new GetPostCommentsQueryHandler(_commentRepository, _postRepository, _commentMapper, _userService);
            var comments = await handler.Handle(request, new CancellationToken());

            return Ok(comments);
        }

        [HttpPost("createComment")]
        public async Task<IActionResult> CreateCommentAsync([FromForm] CreateCommentRequest commentDto)
        {
            var command = new CreateCommentCommand()
            {
                Content = commentDto.Content,
                PostId = commentDto.PostId,
                ParentCommentId = commentDto.ParentCommentId,
            };

            var handler = new CreateCommentCommandHandler(_userService, _postRepository, _commentRepository);

            await handler.Handle(command, new CancellationToken());
            return Ok();
        }

        [HttpDelete("deleteComment")]
        public async Task<IActionResult> DeleteCommentAsync([FromBody] DeleteCommentRequest commentDto)
        {
            var command = new DeleteCommentCommand(new CommentId(commentDto.CommentId));

            var handler = new DeleteCommentCommandHandler(_userService, _commentRepository);
            await handler.Handle(command, new CancellationToken());

            return Ok();
        }

        [HttpPatch("editComment")]
        public async Task<IActionResult> EditCommentAsync([FromBody] EditCommentRequest commentDto)
        {
            var command = new EditCommentCommand()
            {
                CommentId = commentDto.CommentId,
                Content = commentDto.Content,
            };

            var handler = new EditCommentCommandHandler(_userService, _commentRepository);
            await handler.Handle(command, new CancellationToken());

            return Ok();
        }

        [HttpPatch("likeComment")]
        public async Task<IActionResult> LikeCommentAsync([FromBody] LikeCommentRequest commentDto)
        {
            var command = new LikeCommentCommand(new CommentId(commentDto.CommentId));

            var handler = new LikeCommentCommandHandler(_userService, _commentRepository, _commentStatusRepository);
            var result = await handler.Handle(command, new CancellationToken());

            return Ok(result);
        }
    }
}
