using Glowee.Domain.Entities.Users;
using SocialMediaGloweeServer.Data;

namespace Glowee.Persistence.Repositories;

public class UserRepository : GenericRepository<User, UserId>
{
    public UserRepository(SqlDbContext context) : base(context){}
}