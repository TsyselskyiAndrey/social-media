using Glowee.Domain.Common;

namespace Glowee.Domain.Entities
{
    public class Report : BaseEntity<long>
    {
        public bool? IsProcessed { get; set; }
        public int ReportTypeId { get; set; }
        public ReportType ReportType { get; set; }
        public long? ReportedUserId { get; set; }
        public User? ReportedUser { get; set; }
        public long ComplainantId { get; set; }
        public User Complainant { get; set; }
        public long? PostId { get; set; }
        public Post? Post { get; set; }
        public long? CommentId { get; set; }
        public Comment? Comment { get; set; }
    }
}
