using Glowee.Domain.Common;

namespace Glowee.Domain.Entities
{
    public class Follow : BaseEntity<long>
    {
        public long FollowerId { get; set; }
        public User Follower { get; set; }
        public long FollowedId { get; set; }
        public User Followed { get; set; }
    }
}
