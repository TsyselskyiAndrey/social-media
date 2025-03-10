namespace SocialMediaGloweeServer.Models
{
    public class Notification
    {
        public int Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public string Message { get; set; } = String.Empty;
        public bool IsRead { get; set; }
        public int NotificationTypeId { get; set; }
        public NotificationType NotificationType { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public int? SenderId { get; set; }
        public User? Sender { get; set; }
        public int? PostId { get; set; }
        public Post? Post { get; set; }
        public int? CommentId { get; set; }
        public Comment? Comment { get; set; }
    }
}
