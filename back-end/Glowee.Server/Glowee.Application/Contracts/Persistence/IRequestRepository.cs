using Glowee.Domain.Entities.Requests;
using Glowee.Domain.Entities.Users;

namespace Glowee.Application.Contracts.Persistence
{
    public interface IRequestRepository : IGenericRepository<Request, RequestId>
    {
        Task DeleteBySenderIdAsync(UserId userId);
    }
}
