namespace Glowee.Application.Models.Identity
{
    public class AuthResponse
    {
        public long Id { get; set; }
        public string FirstName { get; set; } = String.Empty;
        public string LastName { get; set; } = String.Empty;
        public string UserName { get; set; } = String.Empty;
        public string Email { get; set; } = String.Empty;
        public string? ProfileImageUrl { get; set; }
        public IEnumerable<string> Roles { get; set; } = new List<string>();
        public string Token { get; set; } = String.Empty;
    }
}
