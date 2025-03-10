namespace SocialMediaGloweeServer.Models
{
    public class UserSubscription
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public int SubscriptionId { get; set; }
        public Subscription Subscription { get; set; }
        public DateTime ActivationDate { get; set; }
    }
}
