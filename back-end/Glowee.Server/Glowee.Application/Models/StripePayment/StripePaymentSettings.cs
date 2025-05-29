namespace Glowee.Application.Models.StripePayment
{
    public class StripePaymentSettings
    {
        public string PublishableKey { get; set; } = String.Empty;
        public string SecretKey { get; set; } = String.Empty;
        public string WebhookSecret { get; set; } = String.Empty;

    }
}
