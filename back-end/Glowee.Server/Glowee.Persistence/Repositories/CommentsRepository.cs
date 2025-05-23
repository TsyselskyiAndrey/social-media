using Glowee.Application.Contracts.Persistence;
using Glowee.Domain.Entities.Comments;
using Glowee.Domain.Entities.Posts;
using Glowee.Domain.Entities.Users;
using Glowee.Persistence.DbContext;
using Microsoft.EntityFrameworkCore;

namespace Glowee.Persistence.Repositories;

public class CommentsRepository : GenericRepository<Comment, CommentId> , ICommentsRepository
{
    public CommentsRepository(SqlDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<Comment>> GetIncludedPostComments(PostId postId)
    {
        return await _context.Comments
            .Where(x => x.PostId.Value == postId.Value && x.ParentCommentId == null)
            .AsNoTracking()
            .Include(c => c.ChildComments)
                 .ThenInclude(x => x.CommentStatuses)
            .Include(c => c.ChildComments)
                 .ThenInclude(x => x.User)
            .Include(c => c.CommentStatuses)
            .Include(c => c.User)
            .ToListAsync();
    }
}