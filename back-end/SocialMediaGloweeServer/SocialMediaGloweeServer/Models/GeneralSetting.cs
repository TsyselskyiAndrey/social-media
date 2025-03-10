namespace SocialMediaGloweeServer.Models
{
    public class GeneralSetting
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public bool IsPrivate { get; set; }
        public string Theme { get; set; } = String.Empty;
        public string Language { get; set; } = String.Empty;
    }
}
