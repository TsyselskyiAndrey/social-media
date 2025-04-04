using Glowee.Domain.Entities.Posts;
using SocialMediaGloweeServer.Data;

namespace Glowee.Persistence.Repositories;

public class PostRepository : GenericRepository<Post, PostId>
{
    public PostRepository(SqlDbContext connection) : base(connection) { }
}