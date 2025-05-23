using Glowee.Domain.Common;
using Glowee.Domain.Entities.Comments;
using Glowee.Domain.Entities.Histories;
using Glowee.Domain.Entities.LikedPosts;
using Glowee.Domain.Entities.Notifications;
using Glowee.Domain.Entities.PostMedias;
using Glowee.Domain.Entities.PostTypes;
using Glowee.Domain.Entities.Reports;
using Glowee.Domain.Entities.SavedPosts;
using Glowee.Domain.Entities.Tags;
using Glowee.Domain.Entities.UninterestingPosts;
using Glowee.Domain.Entities.Users;

namespace Glowee.Domain.Entities.Posts
{
    public class Post : BaseEntity<PostId>
    {
        public UserId UserId { get; set; }
        public User User { get; set; }
        public string? Caption { get; set; }
        public PostTypeId PostTypeId { get; set; }
        public PostType PostType { get; set; }
        public ICollection<Tag> Tags { get; set; } = new List<Tag>();
        public ICollection<Report> Reports { get; set; } = new List<Report>();
        public ICollection<Notification> Notifications { get; set; } = new List<Notification>();
        public ICollection<Comment> Comments { get; set; } = new List<Comment>();
        public ICollection<UninterestingPost> UninterestingPosts { get; set; } = new List<UninterestingPost>();
        public ICollection<SavedPost> SavedPosts { get; set; } = new List<SavedPost>();
        public ICollection<PostMedia> PostMedias { get; set; } = new List<PostMedia>();
        public ICollection<LikedPost> LikedPosts { get; set; } = new List<LikedPost>();
        public ICollection<History> Histories { get; set; } = new List<History>();
    }
}
