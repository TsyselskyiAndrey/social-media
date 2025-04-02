namespace Glowee.Application.Models.Identity.LogIn
{
    public class LogInRequest
    {
        public string Login { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string DeviceId { get; set; } = string.Empty;
    }
}
