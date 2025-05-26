using Glowee.Application.Contracts.Persistence;
using Glowee.Domain.Entities.CommentStatuses;
using Glowee.Persistence.DbContext;

namespace Glowee.Persistence.Repositories
{
    public class CommentStatusRepository : GenericRepository<CommentStatus, CommentStatusId>, ICommentStatusRepository
    {
        public CommentStatusRepository(SqlDbContext context) : base(context)
        {
        }
    }
}