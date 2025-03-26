using Glowee.Domain.Common;

namespace Glowee.Domain.Entities
{
    public class RefreshToken : BaseEntity<long>
    {
        public long UserId { get; set; }
        public User User { get; set; }
        public string DeviceId { get; set; } = String.Empty;
        public string Token { get; set; } = String.Empty;
        public DateTime TokenExpiryTime { get; set; }
    }
}
