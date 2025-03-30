using Glowee.Domain.Common;
using Microsoft.AspNetCore.Identity;

namespace Glowee.Identity.Models
{
    public class ApplicationUser : IdentityUser<long>, IEntity
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string? Biography { get; set; }
        public DateTime BannedUntil { get; set; }
        public string? ProfileImageUrl { get; set; }
        public DateTime? BirthDate { get; set; }
        public string? EmailConfirmationCode { get; set; }
        public DateTime EmailConfirmationCodeExpiryTime { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? ModifiedAt { get; set; }
        public ICollection<RefreshToken> RefreshTokens { get; set; } = new List<RefreshToken>();
    }
}
