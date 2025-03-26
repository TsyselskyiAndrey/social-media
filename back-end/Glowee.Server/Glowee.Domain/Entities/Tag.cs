using Glowee.Domain.Common;

namespace Glowee.Domain.Entities
{
    public class Tag : BaseEntity<long>
    {
        public string Name { get; set; } = String.Empty;
        public ICollection<Post> Posts { get; set; } = new List<Post>();
    }
}
