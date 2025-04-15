using Glowee.Application.Contracts.Persistence;
using Glowee.Application.Exceptions;
using Glowee.Domain.Entities.Users;
using Glowee.Persistence.DbContext;

namespace Glowee.Persistence.Repositories
{
    public class UserRepository : GenericRepository<User, UserId>, IUserRepository
    {
        public UserRepository(SqlDbContext context) : base(context)
        { }

        public async Task UpdateProfileImageAsync(UserId userId, string imagePath)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null)
            {
                throw new NotFoundException("User not found");
            }

            user.ProfileImagePath = imagePath;
            await _context.SaveChangesAsync();
        }
    }
}
