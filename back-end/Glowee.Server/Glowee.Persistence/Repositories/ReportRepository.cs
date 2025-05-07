using Glowee.Application.Contracts.Persistence;
using Glowee.Domain.Entities.Reports;
using Glowee.Domain.Entities.Users;
using Glowee.Persistence.DbContext;
using Microsoft.EntityFrameworkCore;

namespace Glowee.Persistence.Repositories
{
    public class ReportRepository : GenericRepository<Report, ReportId>, IReportRepository
    {
        public ReportRepository(SqlDbContext context) : base(context)
        {
        }

        public async Task DeleteByCompliantIdAsync(UserId userId)
        {
            await _context.Reports
                .Where(r => r.ComplainantId == userId)
                .ExecuteDeleteAsync();
        }
    }
}
