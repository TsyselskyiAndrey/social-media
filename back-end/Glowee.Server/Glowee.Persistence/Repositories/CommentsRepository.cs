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
            .Where(x => x.PostId == postId)
            .Include(c => c.ChildComments)
            .Include(c => c.CommentStatuses)
            .Include(c => c.User)
            .AsNoTracking()
            .ToListAsync();
    }
}