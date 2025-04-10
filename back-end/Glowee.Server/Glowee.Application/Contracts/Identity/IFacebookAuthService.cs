using Glowee.Application.Models.Identity.FacebookAuth;

namespace Glowee.Application.Contracts.Identity
{
    public interface IFacebookAuthService
    {
        Task<FacebookTokenValidationResponse> VerifyFacebookToken(string accessToken);
        Task<FacebookUserInfoResponse> GetUserInfoAsync(string accessToken);
    }
}
