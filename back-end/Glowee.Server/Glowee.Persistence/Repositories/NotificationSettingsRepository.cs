using Glowee.Application.Contracts.Persistence;
using Glowee.Domain.Entities.NotificationSettings;
using Glowee.Domain.Entities.Users;
using Glowee.Persistence.DbContext;
using Microsoft.EntityFrameworkCore;

namespace Glowee.Persistence.Repositories;

public class NotificationSettingsRepository : GenericRepository<NotificationSetting, NotificationSettingId>, INotificationSettingsRepository
{
    public NotificationSettingsRepository(SqlDbContext context) : base(context)
    {
    }

    public async Task<NotificationSetting> GetUsersNotificationSettings(UserId id)
    {
        return await _context.NotificationSettings.FirstAsync(x => x.UserId == id);
    }
}