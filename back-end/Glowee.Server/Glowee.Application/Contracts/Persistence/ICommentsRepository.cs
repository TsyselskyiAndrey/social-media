using Glowee.Domain.Entities.Comments;
using Glowee.Domain.Entities.Posts;
using Glowee.Domain.Entities.Users;

namespace Glowee.Application.Contracts.Persistence;

public interface ICommentsRepository : IGenericRepository<Comment, CommentId>
{
    Task DeleteByPostIdAsync(PostId postId);
    Task DeleteByUserIdAsync(UserId userId);
    Task<IEnumerable<Comment>> GetIncludedPostComments(PostId postId);
}