using Glowee.Domain.Common;

namespace Glowee.Domain.Entities
{
    public class MessageAttachment : BaseEntity<long>
    {
        public long MessageId { get; set; }
        public Message Message { get; set; }
        public int MessageAttachmentTypeId { get; set; }
        public MessageAttachmentType MessageAttachmentType { get; set; }
        public string MediaUrl { get; set; } = String.Empty;
        public TimeSpan? Duration { get; set; }
        public string Format { get; set; } = String.Empty;
        public long Size { get; set; }
        public bool? IsUploaded { get; set; }
    }
}
