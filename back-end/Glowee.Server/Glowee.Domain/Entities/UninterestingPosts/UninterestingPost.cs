using Glowee.Domain.Common;
using Glowee.Domain.Entities.Posts;
using Glowee.Domain.Entities.UninterestingPostCauses;
using Glowee.Domain.Entities.Users;

namespace Glowee.Domain.Entities.UninterestingPosts
{
    public class UninterestingPost : BaseEntity<UninterestingPostId>
    {
        public PostId PostId { get; set; }
        public Post Post { get; set; }
        public UserId UserId { get; set; }
        public User User { get; set; }
        public UninterestingPostCauseId CauseId { get; set; }
        public UninterestingPostCause Cause { get; set; }
    }
}
