namespace SocialMediaGloweeServer.Models
{
    public class PostMediaType
    {
        public int Id { get; set; }
        public string Name { get; set; } = String.Empty;
        public ICollection<PostMedia> PostMedias { get; set; } = new List<PostMedia>();
    }
}
