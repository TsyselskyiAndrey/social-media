namespace Glowee.Application.Models.Identity.LogIn
{
    public class LogInResponse
    {
        public AuthResponse AuthResponse { get; set; }
        public RefreshTokenResponse RefreshTokenResponse { get; set; }
    }
}
