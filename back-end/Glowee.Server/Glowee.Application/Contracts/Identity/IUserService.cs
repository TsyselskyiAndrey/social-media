using Glowee.Application.Models.Identity.UserService;
using Glowee.Domain.Entities.Users;

namespace Glowee.Application.Contracts.Identity
{
    public interface IUserService
    {
        public string? UserId { get; }
        Task<UserId> CreateAsync(UserModel userModel);
        Task UpdateAsync(UserModel userModel, UserId userId);
    }
}
