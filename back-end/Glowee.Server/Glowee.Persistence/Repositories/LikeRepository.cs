using Glowee.Application.Contracts.Persistence;
using Glowee.Domain.Entities.LikedPosts;
using SocialMediaGloweeServer.Data;

namespace Glowee.Persistence.Repositories;

public class LikeRepository : GenericRepository<LikedPost, LikedPostId>, ILikeRepository
{
    public LikeRepository(SqlDbContext context): base(context){ }
}