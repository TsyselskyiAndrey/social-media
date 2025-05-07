using Glowee.Application.Contracts.Persistence;
using Glowee.Domain.Entities.Messages;
using Glowee.Domain.Entities.Users;
using Glowee.Persistence.DbContext;
using Microsoft.EntityFrameworkCore;

namespace Glowee.Persistence.Repositories
{
    internal class MessageRepository : GenericRepository<Message, MessageId>, IMessageRepository
    {
        public MessageRepository(SqlDbContext context) : base(context)
        {
        }

        public async Task DeleteBySenderIdAsync(UserId userId)
        {
            await _context.Messages
                .Where(m => m.SenderId == userId)
                .ExecuteDeleteAsync();
        }
    }
}
