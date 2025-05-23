using Glowee.Domain.Common;
using Glowee.Domain.Entities.Subscriptions;
using Glowee.Domain.Entities.Users;

namespace Glowee.Domain.Entities.UserSubscriptions
{
    public class UserSubscription : BaseEntity<UserSubscriptionId>
    {
        public UserId UserId { get; set; }
        public User User { get; set; }
        public SubscriptionId SubscriptionId { get; set; }
        public Subscription Subscription { get; set; }
        public DateTime ActivationDate { get; set; }
    }
}
