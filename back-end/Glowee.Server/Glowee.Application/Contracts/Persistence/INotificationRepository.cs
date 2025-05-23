using Glowee.Domain.Entities.Notifications;
using Glowee.Domain.Entities.Users;

namespace Glowee.Application.Contracts.Persistence
{
    public interface INotificationRepository : IGenericRepository<Notification, NotificationId>
    {
        Task DeleteByUserIdAsync(UserId userId);
    }
}
