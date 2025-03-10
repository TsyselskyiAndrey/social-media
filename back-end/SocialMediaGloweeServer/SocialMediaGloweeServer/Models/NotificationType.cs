namespace SocialMediaGloweeServer.Models
{
    public class NotificationType
    {
        public int Id { get; set; }
        public string Name { get; set; } = String.Empty;
        public ICollection<Notification> Notifications { get; set; } = new List<Notification>();
    }
}
