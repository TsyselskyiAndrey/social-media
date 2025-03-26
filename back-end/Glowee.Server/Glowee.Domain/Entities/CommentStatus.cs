using Glowee.Domain.Common;

namespace Glowee.Domain.Entities
{
    public class CommentStatus : BaseEntity<long>
    {
        public long UserId { get; set; }
        public User User { get; set; }
        public long CommentId { get; set; }
        public Comment Comment { get; set; }
        public bool IsLiked { get; set; }
    }
}
