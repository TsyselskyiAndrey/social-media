using Glowee.Application.Contracts.Identity;
using Glowee.Application.Contracts.Persistence;
using Glowee.Application.Exceptions;
using MediatR;
using UnauthorizedAccessException = Glowee.Application.Exceptions.UnauthorizedAccessException;


namespace Glowee.Application.Features.Comment.Commands.DeleteComment;

public class DeleteCommentCommandHandler : IRequestHandler<DeleteCommentCommand>
{
    private readonly IUserService _userService;
    private readonly ICommentRepository _commentsRepository;
    
    public DeleteCommentCommandHandler(IUserService userService, ICommentRepository commentsRepository)
    {
        _userService = userService;
        _commentsRepository = commentsRepository;
    }
    
    public async Task Handle(DeleteCommentCommand request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(_userService.UserId))
            throw new UnauthorizedAccessException("User must be authenticated to delete comment.");

        var userId = int.Parse(_userService.UserId!);
        
        var comment = await _commentsRepository.GetByIdAsync(request.CommentId);

        if (comment == null)
        {
            throw new NotFoundException("Comment could not be found.");
        }
        
        if (userId != comment.UserId.Value)
        {
            throw new UnauthorizedAccessException("You are not authorized to delete this comment.");
        }
        
        await _commentsRepository.DeleteAsync(comment.Id);
    }
}