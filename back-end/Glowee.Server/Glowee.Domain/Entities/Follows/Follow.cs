using Glowee.Domain.Common;
using Glowee.Domain.Entities.Users;

namespace Glowee.Domain.Entities.Follows
{
    public class Follow : BaseEntity<FollowId>
    {
        public UserId FollowerId { get; set; }
        public User Follower { get; set; }
        public UserId FollowedId { get; set; }
        public User Followed { get; set; }
    }
}
