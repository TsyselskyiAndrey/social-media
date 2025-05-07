using Glowee.Domain.Entities.Reports;
using Glowee.Domain.Entities.Users;

namespace Glowee.Application.Contracts.Persistence
{
    public interface IReportRepository : IGenericRepository<Report, ReportId>
    {
        Task DeleteByCompliantIdAsync(UserId userId);
    }
}
