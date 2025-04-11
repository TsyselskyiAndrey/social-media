using Glowee.Application.Contracts.Persistence;
using Glowee.Domain.Entities.Posts;
using Microsoft.EntityFrameworkCore;
using SocialMediaGloweeServer.Data;

namespace Glowee.Persistence.Repositories;

public class PostRepository : GenericRepository<Post, PostId>, IPostRepository
{
    public PostRepository(SqlDbContext connection) : base(connection) { }
    public void PostExists(PostId id)
    {
        if (!_context.Posts.Any(p => p.Id == id))
        {
            throw new ArgumentException($"Post with id {id} does not exist");
        }
    }

    public async Task<IEnumerable<Post>> GetIncludedPosts()
    {
        return await _context.Posts
                .Include(x => x.LikedPosts)
                .Include(x => x.Tags)
                .Include(x => x.UninterestingPosts)
                .Include(x => x.PostMedias)
                    .ThenInclude(x => x.PostMediaType)
                .AsNoTracking()
                .ToListAsync();
    }
}