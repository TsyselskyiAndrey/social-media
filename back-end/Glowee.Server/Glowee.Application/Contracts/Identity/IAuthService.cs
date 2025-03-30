using Glowee.Application.Models.Identity;

namespace Glowee.Application.Contracts.Identity
{
    public interface IAuthService
    {
        Task<LogInResponse> Login(LogInRequest request);
    }
}
