using Glowee.Domain.Entities.NotificationSettings;
using Glowee.Domain.Entities.Users;

namespace Glowee.Application.Contracts.Persistence;

public interface INotificationSettingsRepository : IGenericRepository<NotificationSetting, NotificationSettingId>
{
    Task<NotificationSetting> GetUsersNotificationSettings(UserId id);
}