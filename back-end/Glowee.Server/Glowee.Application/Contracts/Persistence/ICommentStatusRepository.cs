using Glowee.Domain.Entities.CommentStatuses;

namespace Glowee.Application.Contracts.Persistence
{
    public interface ICommentStatusRepository : IGenericRepository<CommentStatus, CommentStatusId>
    {
    }
}
