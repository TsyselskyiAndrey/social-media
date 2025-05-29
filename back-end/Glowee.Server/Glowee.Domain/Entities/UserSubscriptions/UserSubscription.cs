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
        public string StripeSubscriptionId { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public DateTime StartDate { get; set; }
        public DateTime CurrentPeriodStart { get; set; }
        public DateTime CurrentPeriodEnd { get; set; }
        public DateTime? CancelAt { get; set; }
        public DateTime? CanceledAt { get; set; }
        public bool IsActive { get; set; }
    }
}
