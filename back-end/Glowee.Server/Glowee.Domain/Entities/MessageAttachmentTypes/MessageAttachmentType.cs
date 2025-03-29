using Glowee.Domain.Common;
using Glowee.Domain.Entities.MessageAttachments;

namespace Glowee.Domain.Entities.MessageAttachmentTypes
{
    public class MessageAttachmentType : BaseEntity<MessageAttachmentTypeId>
    {
        public string Name { get; set; } = string.Empty;
        public ICollection<MessageAttachment> MessageAttachments { get; set; } = new List<MessageAttachment>();
    }
}
