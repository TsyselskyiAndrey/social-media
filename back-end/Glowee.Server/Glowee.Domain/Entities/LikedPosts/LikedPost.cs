using Glowee.Domain.Common;
using Glowee.Domain.Entities.Posts;
using Glowee.Domain.Entities.Users;

namespace Glowee.Domain.Entities.LikedPosts
{
    public class LikedPost : BaseEntity<LikedPostId>
    {
        public PostId PostId { get; set; }
        public Post Post { get; set; }
        public UserId UserId { get; set; }
        public User User { get; set; }

    }
}
