using Glowee.Application.Contracts.Persistence;
using Glowee.Domain.Entities.GeneralSettings;
using Glowee.Domain.Entities.Users;
using Glowee.Persistence.DbContext;
using Microsoft.EntityFrameworkCore;

namespace Glowee.Persistence.Repositories;
public class GeneralSettingsRepository : GenericRepository<GeneralSetting, GeneralSettingId>, IGeneralSettingsRepository
{
    public GeneralSettingsRepository(SqlDbContext context) : base(context)
    {
    }

    public async Task<GeneralSetting> GetUsersGeneralSettings(UserId id)
    {
        return await _context.GeneralSettings.FirstAsync(x => x.UserId == id);
    }
}