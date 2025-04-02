using Glowee.Application.Models.Identity.LogIn;

namespace Glowee.Application.Contracts.Identity
{
    public interface IAuthService
    {
        Task<LogInResponse> Login(LogInRequest request);
    }
}
