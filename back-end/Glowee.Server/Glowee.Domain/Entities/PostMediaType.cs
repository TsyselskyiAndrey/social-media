using Glowee.Domain.Common;

namespace Glowee.Domain.Entities
{
    public class PostMediaType : BaseEntity<int>
    {
        public string Name { get; set; } = String.Empty;
        public ICollection<PostMedia> PostMedias { get; set; } = new List<PostMedia>();
    }
}
