using Glowee.Application.Contracts.Identity;
using Glowee.Application.Contracts.Persistence;
using Glowee.Application.Exceptions;
using Glowee.Domain.Entities.Comments;
using Glowee.Domain.Entities.Posts;
using Glowee.Domain.Entities.Users;
using MediatR;
using UnauthorizedAccessException = Glowee.Application.Exceptions.UnauthorizedAccessException;

namespace Glowee.Application.Features.Comment.Commands.CreateComment;

public class CreateCommentCommandHandler : IRequestHandler<CreateCommentCommand>
{
    private readonly IUserService _userService;
    private readonly IPostRepository _postRepository;
    private readonly ICommentRepository _commentsRepository;

    public CreateCommentCommandHandler(IUserService userService,
        IPostRepository postRepository, ICommentRepository commentsRepository)
    {
        _userService = userService;
        _postRepository = postRepository;
        _commentsRepository = commentsRepository;
    }


    public async Task Handle(CreateCommentCommand request, CancellationToken cancellationToken)
    {
        var validator = new CreateCommentCommandValidator();
        var validationResult = await validator.ValidateAsync(request, cancellationToken);

        if (validationResult.Errors.Any())
            throw new BadRequestException("Invalid create comment type", validationResult);

        _postRepository.PostExists(new PostId(request.PostId));

        if (string.IsNullOrEmpty(_userService.UserId))
            throw new UnauthorizedAccessException("User must be authenticated to comment.");

        var userId = long.Parse(_userService.UserId!);

        var newComment = new Domain.Entities.Comments.Comment()
        {
            UserId = new UserId(userId),
            PostId = new PostId(request.PostId),
            Content = request.Content,
        };

        if (request.ParentCommentId != null)
        {
            newComment.ParentCommentId = new CommentId(request.ParentCommentId.Value);
        }

        await _commentsRepository.CreateAsync(newComment);
    }
}