using Glowee.Domain.Common;

namespace Glowee.Domain.Entities
{
    public class ReportType : BaseEntity<int>
    {
        public string Name { get; set; } = String.Empty;
        public ICollection<Report> Reports { get; set; } = new List<Report>();
    }
}
