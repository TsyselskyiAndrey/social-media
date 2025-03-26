using Glowee.Domain.Common;

namespace Glowee.Domain.Entities
{
    public class Post : BaseEntity<long>
    {
        public long UserId { get; set; }
        public User User { get; set; }
        public string? Caption { get; set; }
        public int PostTypeId { get; set; }
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
