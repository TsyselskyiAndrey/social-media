using FluentValidation;
using Glowee.Application.Models.Identity.LogIn;

namespace Glowee.Identity.Validators
{
    public class LogInValidator : AbstractValidator<LogInRequest>
    {
        public LogInValidator()
        {
            RuleFor(r => r.Login)
                   .NotEmpty().WithMessage("Username or Email is required")
                   .NotNull()
                   .EmailAddress().WithMessage("The email address is invalid.")
                   .When(r => r.Login.Contains('@'));

            RuleFor(r => r.Password)
                   .NotEmpty().WithMessage("Password is required.")
                   .MinimumLength(12).WithMessage("Password must be at least 12 characters long.")
                   .MaximumLength(30).WithMessage("Password must not exceed 30 characters.")
                   .Matches(@"[A-Z]").WithMessage("Password must contain at least one uppercase letter.")
                   .Matches(@"[a-z]").WithMessage("Password must contain at least one lowercase letter.")
                   .Matches(@"[0-9]").WithMessage("Password must contain at least one number.")
                   .Matches(@"^[^\s]*$").WithMessage("Password cannot contain spaces.")
                   .Must(password => password.All(c => char.IsLetterOrDigit(c))).WithMessage("Password cannot contain symbols.");

            RuleFor(r => r.DeviceId)
                   .NotEmpty().WithMessage("DeviceId is required.")
                   .NotNull();
        }
    }
}
