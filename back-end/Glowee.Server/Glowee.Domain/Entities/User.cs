using Glowee.Domain.Common;
using Microsoft.AspNetCore.Identity;

namespace Glowee.Domain.Entities
{
    public class User : IdentityUser<long>, IEntity
    {
        public string FirstName { get; set; } = String.Empty;
        public string LastName { get; set; } = String.Empty;
        public string Handle { get; set; } = String.Empty;
        public string? Biography { get; set; } 
        public DateTime BannedUntil { get; set; }
        public string? ProfileImageUrl { get; set; }
        public DateTime? BirthDate { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? ModifiedAt { get; set; }
        public string? EmailConfirmationCode { get; set; }
        public DateTime EmailConfirmationCodeExpiryTime { get; set; }
        public GeneralSetting GeneralSettings { get; set; }
        public NotificationSetting NotificationSettings { get; set; }
        public ICollection<RefreshToken> RefreshTokens { get; set; } = new List<RefreshToken>();
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
