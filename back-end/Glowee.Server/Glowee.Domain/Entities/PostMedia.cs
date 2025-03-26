using Glowee.Domain.Common;

namespace Glowee.Domain.Entities
{
    public class PostMedia : BaseEntity<long>
    {
        public long PostId { get; set; }
        public Post Post { get; set; }
        public string MediaUrl { get; set; } = String.Empty;
        public int PostMediaTypeId { get; set; }
        public PostMediaType PostMediaType { get; set; }
        public string? ThumbnailUrl { get; set; }
        public TimeSpan? Duration { get; set; }
        public string Format { get; set; } = String.Empty;
        public long Size { get; set; }
        public bool? IsUploaded { get; set; }
        public int? Position { get; set; }
    }
}
