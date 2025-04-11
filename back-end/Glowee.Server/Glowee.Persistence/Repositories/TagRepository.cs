using Glowee.Application.Contracts.Persistence;
using Glowee.Domain.Entities.Tags;
using SocialMediaGloweeServer.Data;

namespace Glowee.Persistence.Repositories;

public class TagRepository : GenericRepository<Tag, TagId>, ITagRepository
{
    public TagRepository(SqlDbContext context) : base(context)
    {
    }
}