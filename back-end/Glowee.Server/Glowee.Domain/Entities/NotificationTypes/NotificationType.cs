using Glowee.Domain.Common;
using Glowee.Domain.Entities.Notifications;

namespace Glowee.Domain.Entities.NotificationTypes
{
    public class NotificationType : BaseEntity<NotificationTypeId>
    {
        public string Name { get; set; } = string.Empty;
        public ICollection<Notification> Notifications { get; set; } = new List<Notification>();
    }
}
