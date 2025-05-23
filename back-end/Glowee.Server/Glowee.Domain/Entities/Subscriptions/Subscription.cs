using Glowee.Domain.Common;
using Glowee.Domain.Entities.UserSubscriptions;

namespace Glowee.Domain.Entities.Subscriptions
{
    public class Subscription : BaseEntity<SubscriptionId>
    {
        public string Name { get; set; } = string.Empty;
        public double Price { get; set; }
        public string Description { get; set; } = string.Empty;
        public TimeSpan Duration { get; set; }
        public ICollection<UserSubscription> UserSubscriptions { get; set; } = new List<UserSubscription>();
    }
}
