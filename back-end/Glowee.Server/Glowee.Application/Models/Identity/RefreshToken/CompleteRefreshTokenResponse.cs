namespace Glowee.Application.Models.Identity.RefreshToken
{
    public class CompleteRefreshTokenResponse
    {
        public AuthResponse AuthResponse { get; set; }
        public RefreshTokenResponse RefreshTokenResponse { get; set; }
    }
}
