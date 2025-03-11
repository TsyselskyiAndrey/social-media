namespace SocialMediaGloweeServer.Models
{
    /// <summary>
    /// Represents social network chat
    /// </summary>
    public class Chat
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public DateTime CreatedAt { get; set; }
        public string? LogoUrl { get; set; }
        public bool IsGroup { get; set; }
        public ICollection<UsersChat> UsersChats { get; set; } = new List<UsersChat>();
        public ICollection<Message> Messages { get; set; } = new List<Message>();

    }
}
