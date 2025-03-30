namespace Glowee.Application.Models.Identity
{
    public class LogInRequest
    {
        public string Login { get; set; } = String.Empty;
        public string Password { get; set; } = String.Empty;
        public string DeviceId { get; set; } = String.Empty;
    }
}
