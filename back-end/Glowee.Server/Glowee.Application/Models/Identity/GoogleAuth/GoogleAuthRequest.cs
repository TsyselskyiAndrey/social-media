namespace Glowee.Application.Models.Identity.GoogleAuth
{
    public class GoogleAuthRequest
    {
        public string Provider { get; set; } = String.Empty;
        public string IdToken { get; set; } = String.Empty;
        public string DeviceId { get; set; } = String.Empty;
    }
}
