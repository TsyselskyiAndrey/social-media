using FluentValidation;
using Glowee.Application.Models.Identity.FacebookAuth;

namespace Glowee.Identity.Validators
{
    public class FacebookLoginValidator : AbstractValidator<FacebookAuthRequest>
    {
        public FacebookLoginValidator()
        {
            RuleFor(r => r.AccessToken)
                   .Cascade(CascadeMode.Stop)
                   .NotEmpty().WithMessage("Access token is required");
        }
    }
}
