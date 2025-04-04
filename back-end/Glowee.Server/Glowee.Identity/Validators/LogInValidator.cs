using FluentValidation;
using Glowee.Application.Models.Identity.LogIn;

namespace Glowee.Identity.Validators
{
    public class LogInValidator : AbstractValidator<LogInRequest>
    {
        public LogInValidator()
        {
            RuleFor(r => r.Login)
                   .Cascade(CascadeMode.Stop)
                   .NotEmpty().WithMessage("Username or Email is required")
                   .MinimumLength(3).WithMessage("Username is incorrect.")
                   .When(r => !r.Login.Contains('@'))
                   .MaximumLength(30).WithMessage("Username is incorrect.")
                   .When(r => !r.Login.Contains('@'))
                   .Matches(@"^[a-zA-Z0-9._]+$").WithMessage("Username is incorrect.")
                   .When(r => !r.Login.Contains('@'))
                   .Must(username => !username.Contains("..") && !username.Contains("__")).WithMessage("Username is incorrect.")
                   .When(r => !r.Login.Contains('@'))
                   .EmailAddress().WithMessage("The email address is invalid.")
                   .When(r => r.Login.Contains('@'))
                   .Matches(@"^[^@\s]+@[^@\s]+\.[^@\s]+$").WithMessage("The email address is invalid.")
                   .When(r => r.Login.Contains('@'));

            RuleFor(r => r.Password)
                   .Cascade(CascadeMode.Stop)
                   .NotEmpty().WithMessage("Password is required.")
                   .MinimumLength(12).WithMessage("Incorrect password.")
                   .MaximumLength(30).WithMessage("Incorrect password.")
                   .Matches(@"[A-Z]").WithMessage("Incorrect password.")
                   .Matches(@"[a-z]").WithMessage("Incorrect password.")
                   .Matches(@"[0-9]").WithMessage("Incorrect password.")
                   .Matches(@"^[^\s]*$").WithMessage("Incorrect password.")
                   .Must(password => password.All(c => char.IsLetterOrDigit(c))).WithMessage("Incorrect password.");

            RuleFor(r => r.DeviceId)
                   .Cascade(CascadeMode.Stop)
                   .NotEmpty().WithMessage("DeviceId is required.");
        }
    }
}
