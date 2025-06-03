namespace Glowee.Application.Models.Storage
{
    public class BlobStorageContainerOptions
    {
        public string MessageAttachments { get; set; } = String.Empty;
        public string PostMedia { get; set; } = String.Empty;
        public string ChatLogos { get; set; } = String.Empty;
        public string Thumbnails { get; set; } = String.Empty;
        public string ProfileImages { get; set; } = String.Empty;
        public string Downloads { get; set; } = String.Empty;
    }
}
