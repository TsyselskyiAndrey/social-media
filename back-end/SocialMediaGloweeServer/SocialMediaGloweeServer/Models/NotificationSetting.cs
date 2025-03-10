namespace SocialMediaGloweeServer.Models
{
    public class NotificationSetting
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public bool NotifyPostLikes { get; set; }
        public bool NotifyComments { get; set; }
        public bool NotifyReplies { get; set; }
        public bool NotifyFollows { get; set; }
        public bool NotifyMessages { get; set; }
        public bool NotifyMentions { get; set; }
    }
}
