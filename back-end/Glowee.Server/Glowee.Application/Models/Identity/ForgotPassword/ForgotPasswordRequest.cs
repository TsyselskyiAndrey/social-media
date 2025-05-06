namespace Glowee.Application.Models.Identity.ForgotPassword
{
    public class ForgotPasswordRequest
    {
        public string Email { get; set; } = String.Empty;
        public string ClientUri { get; set; } = String.Empty;
    }
}
