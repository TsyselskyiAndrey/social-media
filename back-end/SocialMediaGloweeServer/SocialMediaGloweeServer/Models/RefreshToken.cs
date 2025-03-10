namespace SocialMediaGloweeServer.Models
{
    public class RefreshToken
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public string DeviceId { get; set; } = String.Empty;
        public string Token { get; set; } = String.Empty;
        public DateTime TokenExpiryTime { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
