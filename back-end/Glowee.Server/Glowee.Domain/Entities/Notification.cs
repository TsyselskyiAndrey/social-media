using Glowee.Domain.Common;

namespace Glowee.Domain.Entities
{
    public class Notification : BaseEntity<long>
    {
        public string Message { get; set; } = String.Empty;
        public bool IsRead { get; set; }
        public int NotificationTypeId { get; set; }
        public NotificationType NotificationType { get; set; }
        public long UserId { get; set; }
        public User User { get; set; }
        public long? SenderId { get; set; }
        public User? Sender { get; set; }
        public long? PostId { get; set; }
        public Post? Post { get; set; }
        public long? CommentId { get; set; }
        public Comment? Comment { get; set; }
    }
}
