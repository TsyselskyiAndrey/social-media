using Glowee.Domain.Common;

namespace Glowee.Domain.Entities
{
    public class NotificationSetting : BaseEntity<long>
    {
        public long UserId { get; set; }
        public User User { get; set; }
        public bool NotifyPostLikes { get; set; }
        public bool NotifyComments { get; set; }
        public bool NotifyReplies { get; set; }
        public bool NotifyFollows { get; set; }
        public bool NotifyMessages { get; set; }
        public bool NotifyMentions { get; set; }
    }
}
