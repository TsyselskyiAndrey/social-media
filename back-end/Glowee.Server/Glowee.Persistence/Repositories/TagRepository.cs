using Glowee.Application.Contracts.Persistence;
using Glowee.Domain.Entities.Tags;
using Glowee.Persistence.DbContext;

namespace Glowee.Persistence.Repositories;

public class TagRepository : GenericRepository<Tag, TagId>, ITagRepository
{
    public TagRepository(SqlDbContext context) : base(context)
    {
    }
}