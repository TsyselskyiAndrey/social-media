using Glowee.Domain.Common;

namespace Glowee.Domain.Entities
{
    public class UserSubscription : BaseEntity<long>
    {
        public long UserId { get; set; }
        public User User { get; set; }
        public int SubscriptionId { get; set; }
        public Subscription Subscription { get; set; }
        public DateTime ActivationDate { get; set; }
    }
}
