using Glowee.Domain.Common;
using Glowee.Domain.Entities.CommentStatuses;
using Glowee.Domain.Entities.Notifications;
using Glowee.Domain.Entities.Posts;
using Glowee.Domain.Entities.Reports;
using Glowee.Domain.Entities.Users;

namespace Glowee.Domain.Entities.Comments
{
    public class Comment : BaseEntity<CommentId>
    {
        public UserId UserId { get; set; }
        public User User { get; set; }
        public PostId PostId { get; set; }
        public Post Post { get; set; }
        public string Content { get; set; } = string.Empty;
        public CommentId? ParentCommentId { get; set; }
        public Comment? ParentComment { get; set; }
        public ICollection<Comment> ChildComments { get; set; } = new List<Comment>();
        public ICollection<CommentStatus> CommentStatuses { get; set; } = new List<CommentStatus>();
        public ICollection<Notification> Notifications { get; set; } = new List<Notification>();
        public ICollection<Report> Reports { get; set; } = new List<Report>();
    }
}
