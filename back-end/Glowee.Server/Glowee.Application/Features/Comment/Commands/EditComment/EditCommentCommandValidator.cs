using FluentValidation;

namespace Glowee.Application.Features.Comment.Commands.EditComment;

public class EditCommentCommandValidator : AbstractValidator<EditCommentCommand>
{
    public EditCommentCommandValidator()
    {
        RuleFor(x => x.CommentId).NotEmpty();
        
        RuleFor(x => x.Content)
            .MaximumLength(2192)
            .NotEmpty()
            .NotNull();
    }
}