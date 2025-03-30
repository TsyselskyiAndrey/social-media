namespace Glowee.Application.Models.Identity
{
    public class RefreshTokenResponse
    {
        public string Token { get; set; } = String.Empty;
        public DateTime ExpiryTime { get; set; }
    }
}
