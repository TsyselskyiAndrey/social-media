using Glowee.Domain.Common;

namespace Glowee.Domain.Entities
{
    public class MessageAttachmentType : BaseEntity<int>
    {
        public string Name { get; set; } = String.Empty;
        public ICollection<MessageAttachment> MessageAttachments { get; set; } = new List<MessageAttachment>();
    }
}
