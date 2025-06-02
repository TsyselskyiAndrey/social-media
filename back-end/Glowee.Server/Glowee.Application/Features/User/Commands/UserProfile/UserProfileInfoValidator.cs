using FluentValidation;
using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Linq;

namespace Glowee.Application.Features.User.Commands.UserProfile;

public class UserProfileInfoValidator : AbstractValidator<UpdateUserProfileInfoCommand>
{
    private readonly string[] _allowedImageExtensions = [".jpg", ".jpeg", ".png", ".webp"];
    private const int MaxImageSizeInBytes = 10 * 1024 * 1024;

    public UserProfileInfoValidator()
    {
        RuleFor(x => x.FirstName)
            .NotEmpty().WithMessage("First name must not be empty")
            .MaximumLength(30).WithMessage("First name must be at most 30 characters long");

        RuleFor(x => x.LastName)
            .NotEmpty().WithMessage("Last name must not be empty")
            .MaximumLength(30).WithMessage("Last name must be at most 30 characters long");

        RuleFor(x => x.Username)
            .NotEmpty().WithMessage("Username is required")
            .Matches(@"^[a-zA-Z0-9._]{3,30}$")
            .WithMessage("Username must be between 3 and 30 characters and can include only letters, numbers, dots or underscores");

        RuleFor(x => x.Biography)
            .MaximumLength(150).WithMessage("Biography must be at most 150 characters long");

        RuleFor(x => x.Birthday)
            .Must(BeAValidDate).WithMessage("Birthday must be a valid date in the past")
            .When(x => x.Birthday.HasValue);

        RuleFor(x => x.ProfilePhoto)
            .Must(BeValidImage).WithMessage("Only .jpg, .jpeg, .png, .webp images up to 5MB are allowed")
            .When(x => x.ProfilePhoto != null);
    }

    private bool BeAValidDate(DateTime? birthday)
    {
        return birthday != null && birthday < DateTime.Today.AddYears(-13);
    }

    private bool BeValidImage(IFormFile? file)
    {
        if (file == null)
            return true;

        var extension = Path.GetExtension(file.FileName).ToLower();
        if (!_allowedImageExtensions.Contains(extension))
            return false;

        if (file.Length > MaxImageSizeInBytes)
            return false;

        return true;
    }
}
