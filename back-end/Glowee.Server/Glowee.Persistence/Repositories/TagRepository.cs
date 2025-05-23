using Glowee.Application.Contracts.Persistence;
using Glowee.Domain.Entities.Tags;
using Glowee.Persistence.DbContext;
using Microsoft.EntityFrameworkCore;

namespace Glowee.Persistence.Repositories;

public class TagRepository : GenericRepository<Tag, TagId>, ITagRepository
{
    public TagRepository(SqlDbContext context) : base(context)
    {
    }

    public async Task<Tag?> GetTagByNameAsync(string name)
    {
        return await _context.Tags.FirstOrDefaultAsync(x => x.Name == name);
    }
}