using Glowee.Domain.Common;

namespace Glowee.Identity.Models
{
    public class RefreshToken : IEntity
    {
        public long Id { get; set; }
        public long UserId { get; set; }
        public AuthUser User { get; set; }
        public string DeviceId { get; set; } = string.Empty;
        public string Token { get; set; } = string.Empty;
        public DateTime ExpiryTime { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? ModifiedAt { get; set; }
    }
}
