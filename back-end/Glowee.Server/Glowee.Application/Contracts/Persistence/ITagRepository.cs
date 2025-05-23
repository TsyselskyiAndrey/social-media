using Glowee.Domain.Entities.Tags;

namespace Glowee.Application.Contracts.Persistence;

public interface ITagRepository : IGenericRepository<Tag, TagId>
{
    Task<Tag?> GetTagByNameAsync(string name);
}