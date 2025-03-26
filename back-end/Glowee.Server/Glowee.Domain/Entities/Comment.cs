using Glowee.Domain.Common;

namespace Glowee.Domain.Entities
{
    public class Comment : BaseEntity<long>
    {
        public long UserId { get; set; }
        public User User { get; set; }
        public long PostId { get; set; }
        public Post Post { get; set; }
        public string Content { get; set; } = String.Empty;
        public long? ParentCommentId { get; set; }
        public Comment? ParentComment { get; set; }
        public ICollection<Comment> ChildComments { get; set; } = new List<Comment>();
        public ICollection<CommentStatus> CommentStatuses { get; set; } = new List<CommentStatus>();
        public ICollection<Notification> Notifications { get; set; } = new List<Notification>();
        public ICollection<Report> Reports { get; set; } = new List<Report>();
    }
}
