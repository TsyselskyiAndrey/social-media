namespace SocialMediaGloweeServer.Models
{
    public class Tag
    {
        public int Id { get; set; }
        public string Name { get; set; } = String.Empty;
        public ICollection<Post> Posts { get; set; } = new List<Post>();
    }
}
