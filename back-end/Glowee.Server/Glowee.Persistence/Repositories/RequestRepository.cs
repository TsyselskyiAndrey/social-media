using Glowee.Application.Contracts.Persistence;
using Glowee.Domain.Entities.Requests;
using Glowee.Domain.Entities.Users;
using Glowee.Persistence.DbContext;
using Microsoft.EntityFrameworkCore;

namespace Glowee.Persistence.Repositories
{
    public class RequestRepository : GenericRepository<Request, RequestId>, IRequestRepository
    {
        public RequestRepository(SqlDbContext context) : base(context)
        {
        }

        public async Task DeleteBySenderIdAsync(UserId userId)
        {
            await _context.Requests
               .Where(r => r.SenderId == userId)
               .ExecuteDeleteAsync();
        }
    }
}
