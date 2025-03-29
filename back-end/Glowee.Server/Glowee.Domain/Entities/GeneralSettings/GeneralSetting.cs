using Glowee.Domain.Common;
using Glowee.Domain.Entities.Users;

namespace Glowee.Domain.Entities.GeneralSettings
{
    public class GeneralSetting : BaseEntity<GeneralSettingId>
    {
        public UserId UserId { get; set; }
        public User User { get; set; }
        public bool IsPrivate { get; set; }
        public string Theme { get; set; } = string.Empty;
        public string Language { get; set; } = string.Empty;
    }
}
