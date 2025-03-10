namespace SocialMediaGloweeServer.Models
{
    public class Request
    {
        public int Id { get; set; }
        public int SenderId { get; set; }
        public User Sender { get; set; }
        public int RecipientId { get; set; }
        public User Recipient { get; set; }
        public int RequestTypeId { get; set; }
        public RequestType RequestType { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool? IsAccepted { get; set; }

    }
}
