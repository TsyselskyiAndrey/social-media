using Glowee.Application.Contracts.Persistence;
using Glowee.Domain.Entities.Notifications;
using Glowee.Domain.Entities.Users;
using Glowee.Persistence.DbContext;
using Microsoft.EntityFrameworkCore;

namespace Glowee.Persistence.Repositories
{
    public class NotificationRepository : GenericRepository<Notification, NotificationId>, INotificationRepository
    {
        public NotificationRepository(SqlDbContext context) : base(context)
        {
        }

        public async Task DeleteByUserIdAsync(UserId userId)
        {
            await _context.Notifications
                .Where(n => n.UserId == userId)
                .ExecuteDeleteAsync();
        }
    }
}
