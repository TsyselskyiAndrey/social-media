using Glowee.Domain.Common;
using Microsoft.AspNetCore.Identity;

namespace Glowee.Identity.Models
{
    public class AuthUser : IdentityUser<long>, IEntity
    {
        public DateTime BannedUntil { get; set; }
        public string? EmailConfirmationCode { get; set; }
        public DateTime EmailConfirmationCodeExpiryTime { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? ModifiedAt { get; set; }
        public ICollection<RefreshToken> RefreshTokens { get; set; } = new List<RefreshToken>();
    }
}
