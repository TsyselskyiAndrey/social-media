using Glowee.Domain.Common;

namespace Glowee.Domain.Entities
{
    public class Message : BaseEntity<long>
    {
        public long SenderId { get; set; }
        public User Sender { get; set; }
        public string Content { get; set; } = String.Empty;
        public long? RecipientId { get; set; }
        public User? Recipient { get; set; }
        public long ChatId { get; set; }
        public Chat Chat { get; set; }
        public ICollection<MessageAttachment> MessageAttachments { get; set; } = new List<MessageAttachment>();
    }
}
