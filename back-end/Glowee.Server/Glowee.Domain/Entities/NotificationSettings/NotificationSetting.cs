using Glowee.Domain.Common;
using Glowee.Domain.Entities.Users;

namespace Glowee.Domain.Entities.NotificationSettings
{
    public class NotificationSetting : BaseEntity<NotificationSettingId>
    {
        public UserId UserId { get; set; }
        public User User { get; set; }
        public bool NotifyPostLikes { get; set; }
        public bool NotifyComments { get; set; }
        public bool NotifyReplies { get; set; }
        public bool NotifyFollows { get; set; }
        public bool NotifyMessages { get; set; }
        public bool NotifyMentions { get; set; }
    }
}
