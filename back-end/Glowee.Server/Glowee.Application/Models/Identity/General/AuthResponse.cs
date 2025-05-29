using Glowee.Application.Models.StripePayment;

namespace Glowee.Application.Models.Identity.General
{
    public class AuthResponse
    {
        public long Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? ProfileImageUrl { get; set; }
        public IEnumerable<string> Roles { get; set; } = new List<string>();
        public IEnumerable<ActiveSubscriptionDto> Subscriptions { get; set; } = new List<ActiveSubscriptionDto>();
        public string Token { get; set; } = string.Empty;
    }
}
