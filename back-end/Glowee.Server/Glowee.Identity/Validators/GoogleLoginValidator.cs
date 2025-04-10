using FluentValidation;
using Glowee.Application.Models.Identity.GoogleAuth;

namespace Glowee.Identity.Validators
{
    public class GoogleLoginValidator : AbstractValidator<GoogleAuthRequest>
    {
        public GoogleLoginValidator()
        {
            RuleFor(r => r.Provider)
                   .Cascade(CascadeMode.Stop)
                   .NotEmpty().WithMessage("Provider is required");

            RuleFor(r => r.IdToken)
                   .Cascade(CascadeMode.Stop)
                   .NotEmpty().WithMessage("IdToken is required");

            RuleFor(r => r.DeviceId)
                   .Cascade(CascadeMode.Stop)
                   .NotEmpty().WithMessage("DeviceId is required");
        }
    }
}
