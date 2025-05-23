using Glowee.Application.Contracts.Persistence;
using Glowee.Application.Features.Post.Queries.SavedPosts;
using Glowee.Domain.Entities.Posts;
using Glowee.Domain.Entities.SavedPosts;
using Glowee.Domain.Entities.Users;
using Glowee.Persistence.DbContext;
using Microsoft.EntityFrameworkCore;

namespace Glowee.Persistence.Repositories;

public class SavedPostRepository : GenericRepository<SavedPost, SavedPostId>, ISavedPostRepository
{
    public SavedPostRepository(SqlDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<Post>> GetUserSavedPostsAsync(UserId userId)
    {
        return await _context.SavedPosts
            .Where(x => x.UserId == userId)
            .AsNoTracking()
            .Include(x => x.Post)
                .ThenInclude(x => x.PostType)
            .Include(x => x.Post)
                .ThenInclude(x => x.LikedPosts)
            .Include(x => x.Post)
                .ThenInclude(x => x.Tags)
            .Include(x => x.Post)
                .ThenInclude(x => x.UninterestingPosts)
            .Include(x => x.Post)
                .ThenInclude(x => x.PostMedias)
                    .ThenInclude(x => x.PostMediaType)
            .Select(x => x.Post)
            .ToListAsync();
    }
}