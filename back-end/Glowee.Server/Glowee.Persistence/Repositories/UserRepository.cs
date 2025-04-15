using Glowee.Application.Contracts.Persistence;
using Glowee.Domain.Entities.Users;
using Glowee.Persistence.DbContext;

namespace Glowee.Persistence.Repositories
{
    public class UserRepository : GenericRepository<User, UserId>, IUserRepository
    {
        public UserRepository(SqlDbContext context) : base(context)
        {
        }
    }
}
