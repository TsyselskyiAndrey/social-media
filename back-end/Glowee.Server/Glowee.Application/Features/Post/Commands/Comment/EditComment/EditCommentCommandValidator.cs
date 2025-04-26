using FluentValidation;

namespace Glowee.Application.Features.Post.Commands.Comment.EditComment;

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