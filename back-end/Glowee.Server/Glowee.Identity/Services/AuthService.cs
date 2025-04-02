using FluentValidation.Results;
using Glowee.Application.Contracts.Identity;
using Glowee.Application.Exceptions;
using Glowee.Application.Models.Identity;
using Glowee.Application.Models.Identity.LogIn;
using Glowee.Application.Models.Identity.Registration;
using Glowee.Identity.DbContext;
using Glowee.Identity.Models;
using Glowee.Identity.Validators;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

using ValidationFailure = FluentValidation.Results.ValidationFailure;

namespace Glowee.Identity.Services
{
    /// <summary>
    /// 
    /// </summary>
    public class AuthService : IAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly JwtSettings _jwtSettings;
        private readonly AuthenticationDbContext _context;

        public AuthService(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IOptions<JwtSettings> jwtSettings, AuthenticationDbContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _jwtSettings = jwtSettings.Value;
            _context = context;
        }

        public async Task<LogInResponse> Login(LogInRequest request)
        {
            var validationResult = await new LogInValidator().ValidateAsync(request);

            if (validationResult.Errors.Any())
            {
                throw new BadRequestException("Invalid login", validationResult);
            }

            var user = request.Login.Contains('@')
                            ? await _userManager.FindByEmailAsync(request.Login)
                            : await _userManager.FindByNameAsync(request.Login);

            if (user == null)
            {
                throw new NotFoundException($"The user ({request.Login}) was not found.");
            }
            if (user.EmailConfirmed == false)
            {
                throw new ForbiddenException("The user hasn't confirmed their email.");
            }

            var result = await _signInManager.CheckPasswordSignInAsync(user, request.Password, false);

            if (result.Succeeded == false)
            {
                var passwordValidationResult = new ValidationResult(new List<ValidationFailure>
                {
                    new ValidationFailure("Password", "Incorrect password.")
                });

                throw new BadRequestException("Invalid login", passwordValidationResult);
            }

            var accessToken = await GenerateAccessToken(user);
            var refreshToken = await GenerateRefreshToken(user, request.DeviceId);

            var authResponse = new AuthResponse
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                UserName = user.UserName ?? "",
                Email = user.Email ?? "",
                ProfileImageUrl = user.ProfileImageUrl,
                Roles = await _userManager.GetRolesAsync(user),
                Token = accessToken
            };
            var refreshTokenResponse = new RefreshTokenResponse
            {
                Token = refreshToken.Token,
                ExpiryTime = refreshToken.ExpiryTime
            };

            return new LogInResponse
            {
                AuthResponse = authResponse,
                RefreshTokenResponse = refreshTokenResponse
            };
        }

        public async Task<RegistrationStep1Response> RegistrationStep1(RegistrationStep1Request request)
        {
            throw new NotImplementedException();
        }

        private async Task<string> GenerateAccessToken(ApplicationUser user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = _jwtSettings.Key;
            if (string.IsNullOrEmpty(key))
            {
                throw new InvalidOperationException("JWT Key is missing from configuration.");
            }
            if (string.IsNullOrEmpty(user.Email) || string.IsNullOrEmpty(user.UserName))
            {
                throw new InvalidOperationException("User's credentials are missing, cannot generate a JWT token.");
            }

            var userClaims = await _userManager.GetClaimsAsync(user);
            var userRoles = await _userManager.GetRolesAsync(user);

            var roleClaims = userRoles.Select(r => new Claim(ClaimTypes.Role, r)).ToList();

            var authClaims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(ClaimTypes.Name, user.UserName)
            }
            .Union(userClaims)
            .Union(roleClaims);

            var keyBytes = Encoding.UTF8.GetBytes(key);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(authClaims),
                Expires = DateTime.UtcNow.AddMinutes(_jwtSettings.AccessTokenValidityInMinutes),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(keyBytes), SecurityAlgorithms.HmacSha256),
                Issuer = _jwtSettings.Issuer,
                Audience = _jwtSettings.Audience
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        private async Task<RefreshToken> GenerateRefreshToken(ApplicationUser user, string deviceId)
        {
            var existingRefreshToken = _context.RefreshTokens
                    .FirstOrDefault(rt => rt.DeviceId == deviceId && rt.UserId == user.Id);

            var randomNumber = new byte[64];
            using (var rng = System.Security.Cryptography.RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
            }

            try
            {
                if (existingRefreshToken != null)
                {
                    existingRefreshToken.Token = Convert.ToBase64String(randomNumber);
                    existingRefreshToken.ExpiryTime = DateTime.UtcNow.AddDays(_jwtSettings.RefreshTokenValidityInDays);
                    _context.Entry(existingRefreshToken).State = EntityState.Modified;
                    await _context.SaveChangesAsync();
                    return existingRefreshToken;
                }
                else
                {
                    var refreshToken = new RefreshToken
                    {
                        UserId = user.Id,
                        User = user,
                        DeviceId = deviceId,
                        Token = Convert.ToBase64String(randomNumber),
                        ExpiryTime = DateTime.UtcNow.AddDays(_jwtSettings.RefreshTokenValidityInDays)
                    };
                    await _context.Entry(user).Collection(u => u.RefreshTokens).LoadAsync();
                    user.RefreshTokens.Add(refreshToken);
                    await _context.SaveChangesAsync();
                    return refreshToken;
                }
            }
            catch (Exception)
            {
                throw new InternalServerException();
            }
        }

        private ClaimsPrincipal? GetPrincipalFromExpiredToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_jwtSettings.Key);

            try
            {
                var validationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = false,
                    IssuerSigningKey = new SymmetricSecurityKey(key)
                };
                var principal = tokenHandler.ValidateToken(token, validationParameters, out var securityToken);
                var jwtSecurityToken = securityToken as JwtSecurityToken;
                if (jwtSecurityToken == null || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                    throw new SecurityTokenException("Invalid token");

                return principal;
            }
            catch
            {
                return null;
            }
        }
    }
}
