using Glowee.Domain.Entities.Users;


namespace Glowee.Application.Contracts.Persistence
{
    public interface IUserRepository : IGenericRepository<User, UserId>
    {

    }
}
