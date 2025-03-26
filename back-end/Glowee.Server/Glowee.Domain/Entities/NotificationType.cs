using Glowee.Domain.Common;

namespace Glowee.Domain.Entities
{
    public class NotificationType : BaseEntity<int>
    {
        public string Name { get; set; } = String.Empty;
        public ICollection<Notification> Notifications { get; set; } = new List<Notification>();
    }
}
