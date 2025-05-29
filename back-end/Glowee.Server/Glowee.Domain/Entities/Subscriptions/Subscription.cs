using Glowee.Domain.Common;
using Glowee.Domain.Entities.UserSubscriptions;

namespace Glowee.Domain.Entities.Subscriptions
{
    public class Subscription : BaseEntity<SubscriptionId>
    {
        public string Name { get; set; } = string.Empty;
        public double Price { get; set; }
        public string StripePriceId { get; set; } = string.Empty;
        public string StripeProductId { get; set; } = string.Empty;
        public string Currency { get; set; } = string.Empty;
        public string Interval { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        public ICollection<UserSubscription> UserSubscriptions { get; set; } = new List<UserSubscription>();
    }
}
