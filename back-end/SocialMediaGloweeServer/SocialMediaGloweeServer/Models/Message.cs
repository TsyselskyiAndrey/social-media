namespace SocialMediaGloweeServer.Models
{
    public class Message
    {
        public int Id { get; set; }
        public int SenderId { get; set; }
        public User Sender { get; set; }
        public string Content { get; set; } = String.Empty;
        public int? RecipientId { get; set; }
        public User? Recipient { get; set; }
        public int ChatId { get; set; }
        public Chat Chat { get; set; }
        public ICollection<MessageAttachment> MessageAttachments { get; set; } = new List<MessageAttachment>();
    }
}
