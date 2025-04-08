using Glowee.Application.Contracts.Persistence;
using Glowee.Domain.Entities.SavedPosts;
using SocialMediaGloweeServer.Data;

namespace Glowee.Persistence.Repositories;

public class SavedPostRepository : GenericRepository<SavedPost, SavedPostId>, ISavedPostRepository
{
    public SavedPostRepository(SqlDbContext context) : base(context)
    {
    }
}