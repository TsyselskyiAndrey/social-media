using Glowee.Domain.Common;

namespace Glowee.Domain.Entities
{
    public class PostType : BaseEntity<int>
    {
        public string Name { get; set; } = String.Empty;
        public ICollection<Post> Posts { get; set; } = new List<Post>();
    }
}
