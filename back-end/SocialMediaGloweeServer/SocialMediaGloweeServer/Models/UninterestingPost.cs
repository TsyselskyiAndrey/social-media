namespace SocialMediaGloweeServer.Models
{
    public class UninterestingPost
    {
        public int Id { get; set; }
        public int PostId { get; set; }
        public Post Post { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public int CauseId { get; set; }
        public UninterestingPostCause Cause { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
