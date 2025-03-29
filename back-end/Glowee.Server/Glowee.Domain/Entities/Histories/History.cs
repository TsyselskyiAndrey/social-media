using Glowee.Domain.Common;
using Glowee.Domain.Entities.Posts;
using Glowee.Domain.Entities.Users;

namespace Glowee.Domain.Entities.Histories
{
    public class History : BaseEntity<HistoryId>
    {
        public PostId PostId { get; set; }
        public Post Post { get; set; }
        public UserId UserId { get; set; }
        public User User { get; set; }
        public TimeSpan? Duration { get; set; }
    }
}
