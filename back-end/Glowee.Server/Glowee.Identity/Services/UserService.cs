using Glowee.Application.Contracts.Identity;
using Glowee.Application.Contracts.Persistence;
using Glowee.Application.Exceptions;
using Glowee.Application.Models.Identity.UserService;
using Glowee.Domain.Entities.Users;
using Glowee.Identity.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Transactions;

namespace Glowee.Identity.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<AuthUser> _userManager;
        private readonly IUserRepository _userRepository;
        private readonly IHttpContextAccessor _contextAccessor;

        public UserService(UserManager<AuthUser> userManager, IUserRepository userRepository, IHttpContextAccessor contextAccessor)
        {
            _userManager = userManager;
            _userRepository = userRepository;
            _contextAccessor = contextAccessor;
        }

        public string? UserId => _contextAccessor.HttpContext?.User?.FindFirstValue(JwtRegisteredClaimNames.Sub);

        public async Task<UserId> CreateAsync(UserModel userModel)
        {
            try
            {
                using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    var newAuthUser = new AuthUser()
                    {
                        UserName = userModel.UserName,
                        Email = userModel.Email,
                        EmailConfirmed = userModel.EmailConfirmed
                    };

                    var createResult = await _userManager.CreateAsync(newAuthUser);

                    if (!createResult.Succeeded)
                    {
                        throw new InternalServerException();
                    }

                    var newUser = new User()
                    {
                        Id = new UserId(newAuthUser.Id),
                        Email = userModel.Email,
                        UserName = userModel.UserName,
                        FirstName = userModel.FirstName,
                        LastName = userModel.LastName,
                        BirthDate = userModel.BirthDate,
                        Biography = userModel.Biography,
                        ProfileImagePath = userModel.ProfileImageUrl
                    };

                    await _userRepository.CreateAsync(newUser);

                    scope.Complete();
                    return new UserId(newAuthUser.Id);
                }
            }
            catch (Exception)
            {
                throw new InternalServerException();
            }

        }

        public async Task UpdateAsync(UserModel userModel, UserId userId)
        {
            try
            {
                using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    var authUser = await _userManager.FindByIdAsync(userId.Value.ToString());

                    if (authUser == null)
                    {
                        throw new NotFoundException("The user wasn't found.");
                    }

                    authUser.Email = userModel.Email;
                    authUser.UserName = userModel.UserName;
                    authUser.EmailConfirmed = userModel.EmailConfirmed;

                    var updateResult = await _userManager.UpdateAsync(authUser);

                    if (!updateResult.Succeeded)
                    {
                        throw new InternalServerException();
                    }

                    var businessUser = await _userRepository.GetByIdAsync(userId);

                    if (businessUser == null)
                    {
                        businessUser = new User()
                        {
                            Id = new UserId(authUser.Id),
                            Email = userModel.Email,
                            UserName = userModel.UserName,
                            FirstName = userModel.FirstName,
                            LastName = userModel.LastName,
                            Biography = userModel.Biography,
                            BirthDate = userModel.BirthDate,
                            ProfileImagePath = userModel.ProfileImageUrl
                        };

                        await _userRepository.CreateAsync(businessUser);
                    }
                    else
                    {
                        businessUser.Email = userModel.Email;
                        businessUser.UserName = userModel.UserName;
                        businessUser.FirstName = userModel.FirstName;
                        businessUser.LastName = userModel.LastName;
                        businessUser.Biography = userModel.Biography;
                        businessUser.BirthDate = userModel.BirthDate;
                        businessUser.ProfileImagePath = userModel.ProfileImageUrl;

                        await _userRepository.UpdateAsync(businessUser);
                    }

                    scope.Complete();
                }
            }
            catch (Exception)
            {
                throw new InternalServerException();
            }
        }
    }
}