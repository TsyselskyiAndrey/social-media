using Glowee.Application.Models.Identity.GoogleAuth;
using Google.Apis.Auth;

namespace Glowee.Application.Contracts.Identity
{
    public interface IGoogleAuthService
    {
        Task<GoogleJsonWebSignature.Payload> VerifyGoogleToken(GoogleAuthRequest authRequest);
    }
}
