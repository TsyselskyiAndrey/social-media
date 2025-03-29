using Glowee.Domain.Common;
using Glowee.Domain.Entities.Comments;
using Glowee.Domain.Entities.Posts;
using Glowee.Domain.Entities.ReportTypes;
using Glowee.Domain.Entities.Users;

namespace Glowee.Domain.Entities.Reports
{
    public class Report : BaseEntity<ReportId>
    {
        public bool? IsProcessed { get; set; }
        public ReportTypeId ReportTypeId { get; set; }
        public ReportType ReportType { get; set; }
        public UserId? ReportedUserId { get; set; }
        public User? ReportedUser { get; set; }
        public UserId ComplainantId { get; set; }
        public User Complainant { get; set; }
        public PostId? PostId { get; set; }
        public Post? Post { get; set; }
        public CommentId? CommentId { get; set; }
        public Comment? Comment { get; set; }
    }
}
