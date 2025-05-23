using Glowee.Domain.Common;
using Glowee.Domain.Entities.Posts;

namespace Glowee.Domain.Entities.Tags
{
    public class Tag : BaseEntity<TagId>
    {
        public string Name { get; set; } = string.Empty;
        public ICollection<Post> Posts { get; set; } = new List<Post>();
    }
}
