namespace SocialMediaGloweeServer.Models
{
    public class LikedPost
    {
        public int Id { get; set; }
        public int PostId { get; set; }
        public Post Post { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public DateTime CreatedAt { get; set; }

    }
}
