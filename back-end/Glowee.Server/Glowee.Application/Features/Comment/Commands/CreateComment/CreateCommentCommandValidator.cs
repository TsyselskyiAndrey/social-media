using FluentValidation;

namespace Glowee.Application.Features.Comment.Commands.CreateComment;

public class CreateCommentCommandValidator : AbstractValidator<CreateCommentCommand>
{
    public CreateCommentCommandValidator()
    {
        RuleFor(x => x.PostId)
            .NotEmpty();

        RuleFor(x => x.Content)
            .MaximumLength(2192)
            .NotEmpty()
            .NotNull();
    }
}