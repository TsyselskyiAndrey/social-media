namespace SocialMediaGloweeServer.Models
{
    public class RequestType
    {
        public int Id { get; set; }
        public string Name { get; set; } = String.Empty;
        public ICollection<Request> Request { get; set; } = new List<Request>();
    }
}
