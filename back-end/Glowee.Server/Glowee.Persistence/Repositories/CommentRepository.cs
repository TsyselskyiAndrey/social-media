using Glowee.Application.Contracts.Persistence;
using Glowee.Application.Exceptions;
using Glowee.Domain.Entities.Comments;
using Glowee.Domain.Entities.Posts;
using Glowee.Domain.Entities.Users;
using Glowee.Persistence.DbContext;
using Microsoft.EntityFrameworkCore;

namespace Glowee.Persistence.Repositories;

public class CommentRepository : GenericRepository<Comment, CommentId>, ICommentRepository
{
    public CommentRepository(SqlDbContext context) : base(context)
    {
    }

    /// <summary>
    /// This is the method that deletes a comment with all its child comments to prevent the cascade errror :)
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public override async Task DeleteAsync(CommentId id)
    {
        var toDelete = new List<Comment>();
        var stack = new Stack<CommentId>();
        stack.Push(id);

        while (stack.Count > 0)
        {
            var currentId = stack.Pop();
            var comment = await _context.Comments.FindAsync(currentId);
            if (comment != null)
            {
                toDelete.Add(comment);
                var childrenIds = await _context.Comments
                    .Where(c => c.ParentCommentId == currentId)
                    .Select(c => c.Id)
                    .ToListAsync();

                foreach (var childId in childrenIds)
                {
                    stack.Push(childId);
                }
            }
        }

        if (toDelete.Count > 0)
        {
            _context.Comments.RemoveRange(toDelete);
            await _context.SaveChangesAsync();
        }
    }

    /// <summary>
    /// The method that deletes all comments below a particular post to avoid the cascade error
    /// </summary>
    /// <param name="postId"></param>
    /// <returns></returns>
    public async Task DeleteByPostIdAsync(PostId postId)
    {
        var rootComments = await _context.Comments
            .Where(c => c.PostId == postId && c.ParentCommentId == null)
            .ToListAsync();

        foreach (var comment in rootComments)
        {
            await DeleteAsync(comment.Id);
        }
    }

    /// <summary>
    /// The method that deletes all comments written by a particular user to avoid the cascade error
    /// </summary>
    /// <param name="userId"></param>
    /// <returns></returns>
    public async Task DeleteByUserIdAsync(UserId userId)
    {
        var userComments = await _context.Comments
            .Where(c => c.UserId == userId)
            .ToListAsync();

        var topLevelComments = userComments
            .Where(c => c.ParentCommentId == null ||
                        !userComments.Any(parent => parent.Id == c.ParentCommentId))
            .ToList();

        foreach (var comment in topLevelComments)
        {
            await DeleteAsync(comment.Id);
        }
    }

    public async Task<IEnumerable<Comment>> GetIncludedPostComments(PostId postId)
    {
        return await _context.Comments
            .Where(c => c.PostId == postId && c.ParentCommentId == null)
            .AsNoTracking()
            .Include(c => c.ChildComments)
                 .ThenInclude(x => x.CommentStatuses)
            .Include(c => c.ChildComments)
                 .ThenInclude(x => x.User)
            .Include(c => c.CommentStatuses)
            .Include(c => c.User)
            .ToListAsync();
    }

    public void CommentExists(CommentId id)
    {
        if (!_context.Comments.Any(c => c.Id == id))
        {
            throw new NotFoundException($"Comment with id {id} does not exist");
        }
    }

    public async Task<Comment?> GetIncludedById(CommentId commentId)
    {
        return await _context.Comments
           .Where(c => c.Id == commentId)
           .AsNoTracking()
           .Include(c => c.ChildComments)
                .ThenInclude(x => x.CommentStatuses)
           .Include(c => c.ChildComments)
                .ThenInclude(x => x.User)
           .Include(c => c.CommentStatuses)
           .Include(c => c.User).FirstOrDefaultAsync();
    }
}