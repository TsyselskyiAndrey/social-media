using System.Text.RegularExpressions;
using FluentValidation;

namespace Glowee.Application.Features.Post.Commands.Post.UpdatePost;

public class EditPostValidator : AbstractValidator<EditPostCommand>
{
    private static readonly Regex ImageRegex = new(@"\.(jpg|jpeg|png|gif)$", RegexOptions.IgnoreCase);
    private static readonly Regex VideoRegex = new(@"\.(mp4|mov|avi|webm)$", RegexOptions.IgnoreCase);

    public EditPostValidator()
    {
        RuleFor(x => x.Caption)
            .MaximumLength(2200)
            .WithMessage("Caption must be 2200 characters or fewer.");

        RuleFor(x => x.PostMedias)
            .NotEmpty().WithMessage("At least one media file is required.")
            .Must(files => files.All(file => IsSupportedMedia(file.FileName)))
            .WithMessage("All files must be supported image or video formats.");

        RuleFor(x => x.Tags)
            .NotEmpty().WithMessage("At least one tag is required.")
            .Must(tags => tags.All(tag => !string.IsNullOrWhiteSpace(tag)))
            .WithMessage("Tags cannot contain empty or whitespace-only strings.");
    }

    private static bool IsSupportedMedia(string fileName)
    {
        var ext = Path.GetExtension(fileName);
        return ImageRegex.IsMatch(ext) || VideoRegex.IsMatch(ext);
    }
}