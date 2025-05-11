using FluentValidation;
using Glowee.Application.Models.Identity.GoogleAuth;

namespace Glowee.Identity.Validators
{
    public class GoogleLoginValidator : AbstractValidator<GoogleAuthRequest>
    {
        public GoogleLoginValidator()
        {
            RuleFor(r => r.CodeOrIdToken)
                   .Cascade(CascadeMode.Stop)
                   .NotEmpty().WithMessage("Code is required");

            RuleFor(r => r.DeviceId)
                   .Cascade(CascadeMode.Stop)
                   .NotEmpty().WithMessage("DeviceId is required");
        }
    }
}
