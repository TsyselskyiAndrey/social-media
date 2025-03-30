namespace Glowee.Application.Models.Identity
{
    public class LogInResponse
    {
        public AuthResponse AuthResponse { get; set; }
        public RefreshTokenResponse RefreshTokenResponse { get; set; }
    }
}
