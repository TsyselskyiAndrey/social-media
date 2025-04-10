using Glowee.Domain.Common;
using Glowee.Domain.Entities.Comments;
using Glowee.Domain.Entities.CommentStatuses;
using Glowee.Domain.Entities.Follows;
using Glowee.Domain.Entities.GeneralSettings;
using Glowee.Domain.Entities.Histories;
using Glowee.Domain.Entities.LikedPosts;
using Glowee.Domain.Entities.Messages;
using Glowee.Domain.Entities.Notifications;
using Glowee.Domain.Entities.NotificationSettings;
using Glowee.Domain.Entities.Posts;
using Glowee.Domain.Entities.Reports;
using Glowee.Domain.Entities.Requests;
using Glowee.Domain.Entities.SavedPosts;
using Glowee.Domain.Entities.UninterestingPosts;
using Glowee.Domain.Entities.UserChats;
using Glowee.Domain.Entities.UserSubscriptions;

namespace Glowee.Domain.Entities.Users
{
    public class User : BaseEntity<UserId>
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public string? Biography { get; set; }
        public string? ProfileImageUrl { get; set; }
        public DateTime? BirthDate { get; set; }
        public GeneralSetting GeneralSettings { get; set; }
        public NotificationSetting NotificationSettings { get; set; }
        public ICollection<Report> ReportsReceived { get; set; } = new List<Report>();
        public ICollection<Report> ReportsMade { get; set; } = new List<Report>();
        public ICollection<Notification> NotificationsSent { get; set; } = new List<Notification>();
        public ICollection<Notification> NotificationsReceived { get; set; } = new List<Notification>();
        public ICollection<Request> RequestsSent { get; set; } = new List<Request>();
        public ICollection<Request> RequestsReceived { get; set; } = new List<Request>();
        public ICollection<UninterestingPost> UninterestingPosts { get; set; } = new List<UninterestingPost>();
        public ICollection<SavedPost> SavedPosts { get; set; } = new List<SavedPost>();
        public ICollection<Post> Posts { get; set; } = new List<Post>();
        public ICollection<LikedPost> LikedPosts { get; set; } = new List<LikedPost>();
        public ICollection<History> Histories { get; set; } = new List<History>();
        public ICollection<CommentStatus> CommentStatuses { get; set; } = new List<CommentStatus>();
        public ICollection<Comment> Comments { get; set; } = new List<Comment>();
        public ICollection<UserSubscription> UserSubscriptions { get; set; } = new List<UserSubscription>();
        public ICollection<Follow> Followings { get; set; } = new List<Follow>();
        public ICollection<Follow> Followers { get; set; } = new List<Follow>();
        public ICollection<UserChat> UsersChats { get; set; } = new List<UserChat>();
        public ICollection<Message> MessagesSent { get; set; } = new List<Message>();
        public ICollection<Message> MessagesReceived { get; set; } = new List<Message>();

    }
}
