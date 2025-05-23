using Glowee.Domain.Common;
using Glowee.Domain.Entities.UninterestingPosts;

namespace Glowee.Domain.Entities.UninterestingPostCauses
{
    public class UninterestingPostCause : BaseEntity<UninterestingPostCauseId>
    {
        public string Name { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public ICollection<UninterestingPost> UninterestingPosts { get; set; } = new List<UninterestingPost>();
    }
}
