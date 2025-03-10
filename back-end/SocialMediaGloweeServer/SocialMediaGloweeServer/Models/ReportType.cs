namespace SocialMediaGloweeServer.Models
{
    public class ReportType
    {
        public int Id { get; set; }
        public string Name { get; set; } = String.Empty;
        public ICollection<Report> Reports { get; set; } = new List<Report>();
    }
}
