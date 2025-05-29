namespace Glowee.Application.Models.StripePayment
{
    public class ActiveSubscriptionDto
    {
        public int SubscriptionId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public double Price { get; set; }
        public string Currency { get; set; } = "EUR";
        public string Interval { get; set; } = "month";
        public DateTime StartDate { get; set; }
        public DateTime CurrentPeriodStart { get; set; }
        public DateTime CurrentPeriodEnd { get; set; }
        public string Status { get; set; } = string.Empty;
    }
}
