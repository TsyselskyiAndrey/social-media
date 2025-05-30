namespace Glowee.Application.Models.StripePayment
{
    public class SubscriptionPlanDto
    {
        public string PlanName { get; set; }
        public string Description { get; set; }
        public string PriceId { get; set; }
        public string ProductId { get; set; }
        public string Price { get; set; }
        public List<string> Features { get; set; }
    }
}
