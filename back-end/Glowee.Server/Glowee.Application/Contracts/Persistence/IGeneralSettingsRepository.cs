using Glowee.Domain.Entities.GeneralSettings;
using Glowee.Domain.Entities.Users;

namespace Glowee.Application.Contracts.Persistence;

public interface IGeneralSettingsRepository : IGenericRepository<GeneralSetting, GeneralSettingId> 
{
    Task<GeneralSetting> GetUsersGeneralSettings(UserId id);
}