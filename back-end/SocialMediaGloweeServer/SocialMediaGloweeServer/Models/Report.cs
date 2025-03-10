namespace SocialMediaGloweeServer.Models
{
    public class Report
    {
        public int Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool? IsProcessed { get; set; }
        public int ReportTypeId { get; set; }
        public ReportType ReportType { get; set; }
        public int? ReportedUserId { get; set; }
        public User? ReportedUser { get; set; }
        public int ComplainantId { get; set; }
        public User Complainant { get; set; }
        public int? PostId { get; set; }
        public Post? Post { get; set; }
        public int? CommentId { get; set; }
        public Comment? Comment { get; set; }
    }
}
