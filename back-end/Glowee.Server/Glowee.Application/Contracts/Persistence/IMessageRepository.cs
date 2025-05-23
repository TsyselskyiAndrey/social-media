using Glowee.Domain.Entities.Messages;
using Glowee.Domain.Entities.Users;

namespace Glowee.Application.Contracts.Persistence
{
    public interface IMessageRepository : IGenericRepository<Message, MessageId>
    {
        Task DeleteBySenderIdAsync(UserId userId);
    }
}
