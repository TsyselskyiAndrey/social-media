using Glowee.Domain.Common;

namespace Glowee.Domain.Entities
{
    public class UninterestingPostCause : BaseEntity<long>
    {
        public string Name { get; set; } = String.Empty;
        public string Message { get; set; } = String.Empty;
        public ICollection<UninterestingPost> UninterestingPosts { get; set; } = new List<UninterestingPost>();
    }
}
