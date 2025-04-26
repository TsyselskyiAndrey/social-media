using Glowee.Application.Contracts.Identity;
using Glowee.Application.Contracts.Persistence;
using Glowee.Application.Exceptions;
using Glowee.Domain.Entities.Comments;
using MediatR;
using UnauthorizedAccessException = Glowee.Application.Exceptions.UnauthorizedAccessException;

namespace Glowee.Application.Features.Post.Commands.Comment.EditComment;

public class EditCommentCommandHandler : IRequestHandler<EditCommentCommand>
{
    private readonly IUserService _userService;
    private readonly ICommentsRepository _commentsRepository;
    
    public EditCommentCommandHandler(IUserService userService, ICommentsRepository commentsRepository)
    {
        _userService = userService;
        _commentsRepository = commentsRepository;
    }
    
    public async Task Handle(EditCommentCommand request, CancellationToken cancellationToken)
    {
        var validator = new EditCommentCommandValidator();
        var validationResult = await validator.ValidateAsync(request, cancellationToken);
        
        if (validationResult.Errors.Any())
            throw new BadRequestException("Invalid create comment type", validationResult);
        
        if (string.IsNullOrEmpty(_userService.UserId))
            throw new UnauthorizedAccessException("User must be authenticated to delete comment.");

        var userId = int.Parse(_userService.UserId!);
        
        var comment = await _commentsRepository.GetByIdAsync(new CommentId(request.CommentId));

        if (comment == null)
        {
            throw new NotFoundException("Comment could not be found.");
        }
        
        if (userId != comment.UserId.Value)
        {
            throw new UnauthorizedAccessException("You are not authorized to edit this comment.");
        }
        
        comment.Content = request.Content;
        await _commentsRepository.UpdateAsync(comment);
    }
}