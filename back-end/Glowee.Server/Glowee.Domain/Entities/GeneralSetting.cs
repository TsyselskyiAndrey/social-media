using Glowee.Domain.Common;

namespace Glowee.Domain.Entities
{
    public class GeneralSetting : BaseEntity<long>
    {
        public long UserId { get; set; }
        public User User { get; set; }
        public bool IsPrivate { get; set; }
        public string Theme { get; set; } = String.Empty;
        public string Language { get; set; } = String.Empty;
    }
}
