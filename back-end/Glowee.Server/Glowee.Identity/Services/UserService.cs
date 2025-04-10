using Glowee.Application.Contracts.Identity;
using Glowee.Application.Contracts.Persistence;
using Glowee.Application.Exceptions;
using Glowee.Application.Models.Identity.UserService;
using Glowee.Domain.Entities.Users;
using Glowee.Identity.DbContext;
using Glowee.Identity.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using SocialMediaGloweeServer.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Glowee.Identity.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<AuthUser> _userManager;
        private readonly IUserRepository _userRepository;
        private readonly AuthenticationDbContext _authContext;
        private readonly SqlDbContext _businessContext;
        private readonly IHttpContextAccessor _contextAccessor;

        public UserService(UserManager<AuthUser> userManager, IUserRepository userRepository, AuthenticationDbContext authContext, SqlDbContext businessContext, IHttpContextAccessor contextAccessor)
        {
            _userManager = userManager;
            _userRepository = userRepository;
            _authContext = authContext;
            _businessContext = businessContext;
            _contextAccessor = contextAccessor;
        }

        public string? UserId => _contextAccessor.HttpContext?.User?.FindFirstValue(JwtRegisteredClaimNames.Sub);

        public async Task<UserId> CreateAsync(UserModel userModel)
        {
            await using var transaction = await _authContext.Database.BeginTransactionAsync();

            try
            {
                var newAuthUser = new AuthUser()
                {
                    UserName = userModel.UserName,
                    Email = userModel.Email,
                    EmailConfirmed = userModel.EmailConfirmed
                };

                var createResult = await _userManager.CreateAsync(newAuthUser);

                if (createResult.Succeeded == false)
                {
                    throw new InternalServerException();
                }

                await _businessContext.Database.UseTransactionAsync(transaction.GetDbTransaction());

                var newUser = new User()
                {
                    Id = new UserId(newAuthUser.Id),
                    Email = userModel.Email,
                    UserName = userModel.UserName,
                    FirstName = userModel.FirstName,
                    LastName = userModel.LastName,
                    BirthDate = userModel.BirthDate,
                    Biography = userModel.Biography,
                    ProfileImageUrl = userModel.ProfileImageUrl
                };

                await _userRepository.CreateAsync(newUser);

                await transaction.CommitAsync();
                return new UserId(newAuthUser.Id);
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                throw new InternalServerException();
            }

        }

        public async Task UpdateAsync(UserModel userModel, UserId userId)
        {
            await using var transaction = await _authContext.Database.BeginTransactionAsync();

            try
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

                if (updateResult.Succeeded == false)
                {
                    throw new InternalServerException();
                }

                await _businessContext.Database.UseTransactionAsync(transaction.GetDbTransaction());

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
                        ProfileImageUrl = userModel.ProfileImageUrl
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
                    businessUser.ProfileImageUrl = userModel.ProfileImageUrl;

                    await _userRepository.UpdateAsync(businessUser);
                }

                await transaction.CommitAsync();
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                throw new InternalServerException();
            }

        }
    }
}
