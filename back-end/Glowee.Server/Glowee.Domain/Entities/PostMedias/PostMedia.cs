using Glowee.Domain.Common;
using Glowee.Domain.Entities.PostMediaTypes;
using Glowee.Domain.Entities.Posts;

namespace Glowee.Domain.Entities.PostMedias
{
    public class PostMedia : BaseEntity<PostMediaId>
    {
        public PostId PostId { get; set; }
        public Post Post { get; set; }
        public string MediaUrl { get; set; } = string.Empty;
        public PostMediaTypeId PostMediaTypeId { get; set; }
        public PostMediaType PostMediaType { get; set; }
        public string? ThumbnailUrl { get; set; }
        public TimeSpan? Duration { get; set; }
        public string Format { get; set; } = string.Empty;
        public long Size { get; set; }
        public bool? IsUploaded { get; set; }
        public int? Position { get; set; }
    }
}
