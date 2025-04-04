using Glowee.Application.Models.Identity.LogIn;
using Glowee.Application.Models.Identity.RefreshToken;
using Glowee.Application.Models.Identity.Registration;
using System.Security.Claims;

namespace Glowee.Application.Contracts.Identity
{
    public interface IAuthService
    {
        Task<LogInResponse> Login(LogInRequest request);
        Task<RegistrationStep1Response> RegistrationStep1(RegistrationStep1Request request);
        Task RegistrationStep2(RegistrationStep2Request request, string? registrationToken);
        Task RegistrationStep3(RegistrationStep3Request request, string? registrationToken);
        Task<CompleteRefreshTokenResponse> RefreshToken(RefreshTokenRequest request, string? refreshToken);
        Task Logout(ClaimsPrincipal userPrincipal);
    }
}
