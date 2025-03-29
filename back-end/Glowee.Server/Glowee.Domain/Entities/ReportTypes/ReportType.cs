using Glowee.Domain.Common;
using Glowee.Domain.Entities.Reports;

namespace Glowee.Domain.Entities.ReportTypes
{
    public class ReportType : BaseEntity<ReportTypeId>
    {
        public string Name { get; set; } = string.Empty;
        public ICollection<Report> Reports { get; set; } = new List<Report>();
    }
}
