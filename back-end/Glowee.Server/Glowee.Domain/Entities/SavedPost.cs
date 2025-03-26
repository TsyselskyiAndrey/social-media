using Glowee.Domain.Common;

namespace Glowee.Domain.Entities
{
    public class SavedPost : BaseEntity<long>
    {
        public long PostId { get; set; }
        public Post Post { get; set; }
        public long UserId { get; set; }
        public User User { get; set; }
    }
}
