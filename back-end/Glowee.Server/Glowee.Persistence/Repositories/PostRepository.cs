using Glowee.Application.Contracts.Persistence;
using Glowee.Application.Exceptions;
using Glowee.Domain.Entities.Posts;
using Glowee.Domain.Entities.Users;
using Glowee.Persistence.DbContext;
using Microsoft.EntityFrameworkCore;

namespace Glowee.Persistence.Repositories;

public class PostRepository : GenericRepository<Post, PostId>, IPostRepository
{
    ICommentsRepository _commentsRepository;

    public PostRepository(SqlDbContext connection, ICommentsRepository commentsRepository) : base(connection)
    {
        _commentsRepository = commentsRepository;
    }

    public override async Task DeleteAsync(PostId id)
    {
        await _commentsRepository.DeleteByPostIdAsync(id);
        await base.DeleteAsync(id);
    }

    public async Task DeleteByUserIdAsync(UserId userId)
    {
        var posts = await _context.Posts
            .Where(c => c.UserId == userId)
            .ToListAsync();

        foreach (var post in posts)
        {
            await DeleteAsync(post.Id);
        }
    }

    public void PostExists(PostId id)
    {
        if (!_context.Posts.Any(p => p.Id == id))
        {
            throw new NotFoundException($"Post with id {id} does not exist");
        }
    }

    public async Task<IEnumerable<Post>> GetIncludedPosts()
    {
        return await _context.Posts
                .Include(x => x.LikedPosts)
                .Include(x => x.Tags)
                .Include(x => x.PostType)
                .Include(x => x.UninterestingPosts)
                .Include(x => x.PostMedias)
                    .ThenInclude(x => x.PostMediaType)
                .AsNoTracking()
                .ToListAsync();
    }
}