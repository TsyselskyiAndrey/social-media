using Glowee.Domain.Entities.Comments;
using Glowee.Domain.Entities.Posts;

namespace Glowee.Application.Contracts.Persistence;

public interface ICommentsRepository : IGenericRepository<Comment, CommentId>
{
    Task<IEnumerable<Comment>> GetIncludedPostComments(PostId postId);
}