using Glowee.Domain.Common;
using Glowee.Domain.Entities.Chats;
using Glowee.Domain.Entities.MessageAttachments;
using Glowee.Domain.Entities.Users;

namespace Glowee.Domain.Entities.Messages
{
    public class Message : BaseEntity<MessageId>
    {
        public UserId SenderId { get; set; }
        public User Sender { get; set; }
        public string Content { get; set; } = string.Empty;
        public UserId? RecipientId { get; set; }
        public User? Recipient { get; set; }
        public ChatId ChatId { get; set; }
        public Chat Chat { get; set; }
        public ICollection<MessageAttachment> MessageAttachments { get; set; } = new List<MessageAttachment>();
    }
}
