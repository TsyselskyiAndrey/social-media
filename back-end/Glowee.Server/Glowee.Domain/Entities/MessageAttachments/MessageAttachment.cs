using Glowee.Domain.Common;
using Glowee.Domain.Entities.MessageAttachmentTypes;
using Glowee.Domain.Entities.Messages;

namespace Glowee.Domain.Entities.MessageAttachments
{
    public class MessageAttachment : BaseEntity<MessageAttachmentId>
    {
        public MessageId MessageId { get; set; }
        public Message Message { get; set; }
        public MessageAttachmentTypeId MessageAttachmentTypeId { get; set; }
        public MessageAttachmentType MessageAttachmentType { get; set; }
        public string MediaUrl { get; set; } = string.Empty;
        public TimeSpan? Duration { get; set; }
        public string Format { get; set; } = string.Empty;
        public long Size { get; set; }
        public bool? IsUploaded { get; set; }
    }
}
