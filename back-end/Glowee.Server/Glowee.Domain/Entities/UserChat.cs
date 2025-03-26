using Glowee.Domain.Common;

namespace Glowee.Domain.Entities
{
    public class UserChat : BaseEntity<long>
    {
        public long UserId { get; set; }
        public User User { get; set; }
        public long ChatId { get; set; }
        public Chat Chat { get; set; }
        public bool? IsAdminOrModerator { get; set; }
    }
}
