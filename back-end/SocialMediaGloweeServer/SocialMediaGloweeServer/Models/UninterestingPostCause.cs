namespace SocialMediaGloweeServer.Models
{
    public class UninterestingPostCause
    {
        public int Id { get; set; }
        public string Name { get; set; } = String.Empty;
        public ICollection<UninterestingPost> UninterestingPosts { get; set; } = new List<UninterestingPost>();
    }
}
