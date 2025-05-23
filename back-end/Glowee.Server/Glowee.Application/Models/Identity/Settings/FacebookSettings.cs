namespace Glowee.Application.Models.Identity.Settings
{
    public class FacebookSettings
    {
        public string BaseUrl { get; set; } = String.Empty;
        public string TokenValidationUrl { get; set; } = String.Empty;
        public string UserInfoUrl { get; set; } = String.Empty;
        public string AppId { get; set; } = String.Empty;
        public string AppSecret { get; set; } = String.Empty;
    }
}
