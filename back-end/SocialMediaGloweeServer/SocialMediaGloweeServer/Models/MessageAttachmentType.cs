namespace SocialMediaGloweeServer.Models
{
    public class MessageAttachmentType
    {
        public int Id { get; set; }
        public string Name { get; set; } = String.Empty;
        public ICollection<MessageAttachment> MessageAttachments { get; set; } = new List<MessageAttachment>();
    }
}
