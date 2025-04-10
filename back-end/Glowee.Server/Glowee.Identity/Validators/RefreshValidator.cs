using FluentValidation;
using Glowee.Application.Models.Identity.RefreshToken;

namespace Glowee.Identity.Validators
{
    public class RefreshValidator : AbstractValidator<RefreshTokenRequest>
    {
        public RefreshValidator()
        {
            RuleFor(r => r.AccessToken)
                   .Cascade(CascadeMode.Stop)
                   .NotEmpty().WithMessage("Access token is required");
        }
    }
}
