using Glowee.Domain.Common;

namespace Glowee.Domain.Entities
{
    public class Subscription : BaseEntity<int>
    {
        public string Name { get; set; } = String.Empty;
        public double Price { get; set; }
        public string Description { get; set; } = String.Empty;
        public TimeSpan Duration { get; set; }
        public ICollection<UserSubscription> UserSubscriptions { get; set; } = new List<UserSubscription>();
    }
}
