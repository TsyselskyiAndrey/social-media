using Glowee.Domain.Common;
using Glowee.Domain.Entities.PostMedias;

namespace Glowee.Domain.Entities.PostMediaTypes
{
    public class PostMediaType : BaseEntity<PostMediaTypeId>
    {
        public string Name { get; set; } = string.Empty;
        public ICollection<PostMedia> PostMedias { get; set; } = new List<PostMedia>();
    }
}
