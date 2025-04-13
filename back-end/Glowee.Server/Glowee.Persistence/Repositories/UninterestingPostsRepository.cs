using Glowee.Application.Contracts.Persistence;
using Glowee.Domain.Entities.UninterestingPosts;
using Glowee.Persistence.DbContext;

namespace Glowee.Persistence.Repositories;

public class UninterestingPostsRepository : GenericRepository<UninterestingPost, UninterestingPostId>, IUninterestingPostRepository
{
    public UninterestingPostsRepository(SqlDbContext context) : base(context)
    {
    }
}