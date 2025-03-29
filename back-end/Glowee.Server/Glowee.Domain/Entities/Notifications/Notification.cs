using Glowee.Domain.Common;
using Glowee.Domain.Entities.Comments;
using Glowee.Domain.Entities.NotificationTypes;
using Glowee.Domain.Entities.Posts;
using Glowee.Domain.Entities.Users;

namespace Glowee.Domain.Entities.Notifications
{
    public class Notification : BaseEntity<NotificationId>
    {
        public string Message { get; set; } = string.Empty;
        public bool IsRead { get; set; }
        public NotificationTypeId NotificationTypeId { get; set; }
        public NotificationType NotificationType { get; set; }
        public UserId UserId { get; set; }
        public User User { get; set; }
        public UserId? SenderId { get; set; }
        public User? Sender { get; set; }
        public PostId? PostId { get; set; }
        public Post? Post { get; set; }
        public CommentId? CommentId { get; set; }
        public Comment? Comment { get; set; }
    }
}
