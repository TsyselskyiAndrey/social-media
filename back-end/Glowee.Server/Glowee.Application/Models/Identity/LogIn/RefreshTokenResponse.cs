namespace Glowee.Application.Models.Identity.LogIn
{
    public class RefreshTokenResponse
    {
        public string Token { get; set; } = string.Empty;
        public DateTime ExpiryTime { get; set; }
    }
}
