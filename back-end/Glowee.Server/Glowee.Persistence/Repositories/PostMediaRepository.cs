using Glowee.Application.Contracts.Persistence;
using Glowee.Domain.Entities.PostMedias;
using Glowee.Persistence.DbContext;

namespace Glowee.Persistence.Repositories;

public class PostMediaRepository : GenericRepository<PostMedia, PostMediaId>, IPostMediaRepository
{
    public PostMediaRepository(SqlDbContext context) : base(context)
    {
    }
}