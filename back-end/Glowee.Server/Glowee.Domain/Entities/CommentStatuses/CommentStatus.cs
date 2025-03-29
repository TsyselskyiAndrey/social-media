using Glowee.Domain.Common;
using Glowee.Domain.Entities.Comments;
using Glowee.Domain.Entities.Users;

namespace Glowee.Domain.Entities.CommentStatuses
{
    public class CommentStatus : BaseEntity<CommentStatusId>
    {
        public UserId UserId { get; set; }
        public User User { get; set; }
        public CommentId CommentId { get; set; }
        public Comment Comment { get; set; }
        public bool IsLiked { get; set; }
    }
}
