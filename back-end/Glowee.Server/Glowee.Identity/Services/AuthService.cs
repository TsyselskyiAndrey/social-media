using FluentValidation.Results;
using Glowee.Application.Contracts.Email;
using Glowee.Application.Contracts.Identity;
using Glowee.Application.Contracts.Persistence;
using Glowee.Application.Contracts.Storage;
using Glowee.Application.Exceptions;
using Glowee.Application.Models.Email;
using Glowee.Application.Models.Identity.FacebookAuth;
using Glowee.Application.Models.Identity.ForgotPassword;
using Glowee.Application.Models.Identity.General;
using Glowee.Application.Models.Identity.GoogleAuth;
using Glowee.Application.Models.Identity.LogIn;
using Glowee.Application.Models.Identity.RefreshToken;
using Glowee.Application.Models.Identity.Registration;
using Glowee.Application.Models.Identity.Settings;
using Glowee.Application.Models.Identity.UserService;
using Glowee.Application.Models.Storage;
using Glowee.Domain.Entities.Users;
using Glowee.Identity.DbContext;
using Glowee.Identity.Models;
using Glowee.Identity.Validators;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using UnauthorizedAccessException = Glowee.Application.Exceptions.UnauthorizedAccessException;
using ValidationFailure = FluentValidation.Results.ValidationFailure;

namespace Glowee.Identity.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<AuthUser> _userManager;
        private readonly SignInManager<AuthUser> _signInManager;
        private readonly JwtSettings _jwtSettings;
        private readonly AuthSettings _authSettings;
        private readonly DefaultFiles _defaultFiles;
        private readonly AuthenticationDbContext _context;
        private readonly IUserRepository _userRepository;
        private readonly IEmailSender _emailSender;
        private readonly IUserService _userService;
        private readonly IGoogleAuthService _googleAuthService;
        private readonly IFacebookAuthService _facebookAuthService;
        private readonly IProfileImageStorageService _profileImageStorageService;

        public AuthService(UserManager<AuthUser> userManager, SignInManager<AuthUser> signInManager, IOptions<JwtSettings> jwtSettings, IOptions<AuthSettings> authSettings, IOptions<DefaultFiles> defaultFiles, AuthenticationDbContext context, IUserRepository userRepository, IEmailSender emailSender, IUserService userService, IGoogleAuthService googleAuthService, IFacebookAuthService facebookAuthService, IProfileImageStorageService profileImageStorageService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _jwtSettings = jwtSettings.Value;
            _authSettings = authSettings.Value;
            _defaultFiles = defaultFiles.Value;
            _context = context;
            _userRepository = userRepository;
            _emailSender = emailSender;
            _userService = userService;
            _googleAuthService = googleAuthService;
            _facebookAuthService = facebookAuthService;
            _profileImageStorageService = profileImageStorageService;
        }

        public async Task<CompleteAuthResponse> FacebookLogin(FacebookAuthRequest request)
        {
            var validationResult = await new FacebookLoginValidator().ValidateAsync(request);

            if (validationResult.Errors.Count != 0)
            {
                throw new BadRequestException("Invalid facebook login.", validationResult);
            }

            var validatedTokenResult = await _facebookAuthService.VerifyFacebookToken(request.AccessToken);

            if (validatedTokenResult.Data.IsValid == false)
            {
                throw new BadRequestException("Access token is invalid.");
            }

            var userInfo = await _facebookAuthService.GetUserInfoAsync(request.AccessToken);

            if (string.IsNullOrWhiteSpace(userInfo.Email))
            {
                throw new BadRequestException("Facebook account has no accessible email address.");
            }

            var info = new UserLoginInfo("Facebook", userInfo.Id, "Facebook");
            var authUser = await _userManager.FindByLoginAsync(info.LoginProvider, info.ProviderKey);

            if (authUser == null)
            {
                authUser = await _userManager.FindByEmailAsync(userInfo.Email);
                if (authUser == null)
                {
                    var userModel = new UserModel
                    {
                        Email = userInfo.Email,
                        UserName = userInfo.Email,
                        EmailConfirmed = true,
                        FirstName = userInfo.FirstName,
                        LastName = userInfo.LastName,
                        BirthDate = null,
                        Biography = null,
                        ProfileImageUrl = _defaultFiles.DefaultProfilePicture
                    };

                    var userId = await _userService.CreateAsync(userModel);
                    authUser = await _userManager.FindByIdAsync(userId.Value.ToString());

                    if (authUser == null)
                    {
                        throw new InternalServerException();
                    }

                    await _userManager.AddLoginAsync(authUser, info);
                    await TrySetExternalProfileImageAsync(userId, userInfo.Picture.Data.Url.AbsoluteUri, "facebook_profile.jpg");
                }
                else if (authUser.EmailConfirmed == false)
                {
                    var userModel = new UserModel
                    {
                        Email = userInfo.Email,
                        UserName = userInfo.Email,
                        EmailConfirmed = true,
                        FirstName = userInfo.FirstName,
                        LastName = userInfo.LastName,
                        BirthDate = null,
                        Biography = null,
                        ProfileImageUrl = _defaultFiles.DefaultProfilePicture
                    };

                    await _userService.UpdateAsync(userModel, new UserId(authUser.Id));
                    await _userManager.AddLoginAsync(authUser, info);
                    await TrySetExternalProfileImageAsync(new UserId(authUser.Id), userInfo.Picture.Data.Url.AbsoluteUri, "facebook_profile.jpg");
                }
                else
                {
                    await _userManager.AddLoginAsync(authUser, info);
                }
            }
            if (authUser == null)
            {
                throw new BadRequestException("Invalid External Authentication.");
            }

            return await GenerateAuthResponse(authUser, request.DeviceId);
        }

        public async Task<CompleteAuthResponse> GoogleLogin(GoogleAuthRequest request)
        {
            var validationResult = await new GoogleLoginValidator().ValidateAsync(request);

            if (validationResult.Errors.Count != 0)
            {
                throw new BadRequestException("Invalid google login.", validationResult);
            }

            var payload = await _googleAuthService.VerifyGoogleToken(request);
            if (payload == null)
            {
                throw new BadRequestException("Invalid External Authentication.");
            }
            if (payload.EmailVerified == false)
            {
                throw new BadRequestException("Google email is not verified.");
            }
            var info = new UserLoginInfo("Google", payload.Subject, "Google");
            var authUser = await _userManager.FindByLoginAsync(info.LoginProvider, info.ProviderKey);

            if (authUser == null)
            {
                authUser = await _userManager.FindByEmailAsync(payload.Email);
                if (authUser == null)
                {
                    var userModel = new UserModel
                    {
                        Email = payload.Email,
                        UserName = payload.Email,
                        EmailConfirmed = true,
                        FirstName = payload.GivenName,
                        LastName = payload.FamilyName,
                        BirthDate = null,
                        Biography = null,
                        ProfileImageUrl = _defaultFiles.DefaultProfilePicture
                    };

                    var userId = await _userService.CreateAsync(userModel);
                    authUser = await _userManager.FindByIdAsync(userId.Value.ToString());

                    if (authUser == null)
                    {
                        throw new InternalServerException();
                    }

                    await _userManager.AddLoginAsync(authUser, info);
                    await TrySetExternalProfileImageAsync(userId, payload.Picture, "google_profile.jpg");
                }
                else if (authUser.EmailConfirmed == false)
                {
                    var userModel = new UserModel
                    {
                        Email = payload.Email,
                        UserName = payload.Email,
                        EmailConfirmed = true,
                        FirstName = payload.GivenName,
                        LastName = payload.FamilyName,
                        BirthDate = null,
                        Biography = null,
                        ProfileImageUrl = _defaultFiles.DefaultProfilePicture
                    };

                    await _userService.UpdateAsync(userModel, new UserId(authUser.Id));
                    await _userManager.AddLoginAsync(authUser, info);
                    await TrySetExternalProfileImageAsync(new UserId(authUser.Id), payload.Picture, "google_profile.jpg");
                }
                else
                {
                    await _userManager.AddLoginAsync(authUser, info);
                }
            }
            if (authUser == null)
            {
                throw new BadRequestException("Invalid External Authentication.");
            }

            return await GenerateAuthResponse(authUser, request.DeviceId);
        }

        public async Task<CompleteAuthResponse> Login(LogInRequest request)
        {
            var validationResult = await new LogInValidator().ValidateAsync(request);

            if (validationResult.Errors.Count != 0)
            {
                throw new BadRequestException("Invalid login", validationResult);
            }

            var authUser = request.Login.Contains('@')
                            ? await _userManager.FindByEmailAsync(request.Login)
                            : await _userManager.FindByNameAsync(request.Login);

            if (authUser == null)
            {
                throw new NotFoundException($"The user ({request.Login}) was not found.");
            }
            if (authUser.EmailConfirmed == false)
            {
                throw new ForbiddenException("The email is not confirmed. Finish your registration");
            }
            if (string.IsNullOrWhiteSpace(authUser.PasswordHash))
            {
                var passwordValidationResult = new ValidationResult(new List<ValidationFailure>
                {
                    new ValidationFailure("Password", "The account was registered with no password. Try another way")
                });

                throw new BadRequestException("Invalid login", passwordValidationResult);
            }

            var result = await _signInManager.CheckPasswordSignInAsync(authUser, request.Password, false);

            if (result.Succeeded == false)
            {
                var passwordValidationResult = new ValidationResult(new List<ValidationFailure>
                {
                    new ValidationFailure("Password", "Incorrect password.")
                });

                throw new BadRequestException("Invalid login", passwordValidationResult);
            }

            return await GenerateAuthResponse(authUser, request.DeviceId);
        }

        public async Task<RegistrationStep1Response> RegistrationStep1(RegistrationStep1Request request)
        {
            var validationResult = await new RegistrationStep1Validator().ValidateAsync(request);

            if (validationResult.Errors.Count != 0)
            {
                throw new BadRequestException("Invalid registration", validationResult);
            }

            var existingUser = await _userManager.FindByNameAsync(request.UserName);

            if (existingUser != null && existingUser.EmailConfirmed)
            {
                var userNameValidationResult = new ValidationResult(new List<ValidationFailure>
                {
                    new ValidationFailure("UserName", "This username has been taken.")
                });

                throw new BadRequestException("Invalid registration", userNameValidationResult);
            }

            if (existingUser == null)
            {
                existingUser = await _userManager.FindByEmailAsync(request.Email);

                if (existingUser != null && existingUser.EmailConfirmed)
                {
                    var emailValidationResult = new ValidationResult(new List<ValidationFailure>
                    {
                        new ValidationFailure("Email", "This email has been taken.")
                    });

                    throw new BadRequestException("Invalid registration", emailValidationResult);
                }
            }


            string registrationToken = "";

            if (existingUser != null)
            {
                existingUser.Email = request.Email;
                existingUser.UserName = request.UserName;
                var passwordResetToken = await _userManager.GeneratePasswordResetTokenAsync(existingUser);
                var setPasswordResult = await _userManager.ResetPasswordAsync(existingUser, passwordResetToken, request.Password);
                if (!setPasswordResult.Succeeded)
                {
                    throw new InternalServerException();
                }

                var updateResult = await _userManager.UpdateAsync(existingUser);
                if (!updateResult.Succeeded)
                {
                    throw new InternalServerException();
                }

                registrationToken = await GenerateJwtToken(existingUser, _jwtSettings.RegistrationTokenValidityInMinutes);
            }
            else
            {
                var newUser = new AuthUser()
                {
                    UserName = request.UserName,
                    Email = request.Email
                };

                var createResult = await _userManager.CreateAsync(newUser, request.Password);

                if (createResult.Succeeded == false)
                {
                    throw new InternalServerException();
                }

                registrationToken = await GenerateJwtToken(newUser, _jwtSettings.RegistrationTokenValidityInMinutes);
            }

            return new RegistrationStep1Response
            {
                Token = registrationToken,
                ExpiryTime = DateTime.UtcNow.AddMinutes(_jwtSettings.RegistrationTokenValidityInMinutes),
            };
        }

        public async Task RegistrationStep2(RegistrationStep2Request request, string? registrationToken)
        {
            var validationResult = await new RegistrationStep2Validator().ValidateAsync(request);

            if (validationResult.Errors.Count != 0)
            {
                throw new BadRequestException("Invalid registration", validationResult);
            }

            if (string.IsNullOrWhiteSpace(registrationToken))
            {
                throw new UnauthorizedAccessException("Token is missing");
            }
            var principal = ExtractUserPrincipalFromToken(registrationToken);

            string? email = principal.FindFirst(ClaimTypes.Email)?.Value;
            if (string.IsNullOrWhiteSpace(email))
            {
                throw new UnauthorizedAccessException("Invalid registration token");
            }

            var authUser = await _userManager.FindByEmailAsync(email);

            if (authUser == null)
            {
                throw new NotFoundException($"The user ('{email}') wasn't found.");
            }
            if (authUser.EmailConfirmed)
            {
                throw new InternalServerException();
            }

            var user = await _userRepository.GetByIdAsync(new UserId(authUser.Id));

            if (user == null)
            {
                var newUser = new User()
                {
                    Id = new UserId(authUser.Id),
                    Email = authUser.Email ?? "",
                    UserName = authUser.UserName ?? "",
                    FirstName = request.FirstName,
                    LastName = request.LastName,
                    BirthDate = DateTime.ParseExact(request.BirthDate, "MM/dd/yyyy", CultureInfo.InvariantCulture),
                    ProfileImagePath = _defaultFiles.DefaultProfilePicture
                };

                try
                {
                    await _userRepository.CreateAsync(newUser);
                }
                catch (Exception)
                {
                    throw new InternalServerException();
                }
            }
            else
            {
                user.FirstName = request.FirstName;
                user.LastName = request.LastName;
                user.BirthDate = DateTime.ParseExact(request.BirthDate, "MM/dd/yyyy", CultureInfo.InvariantCulture);

                try
                {
                    await _userRepository.UpdateAsync(user);
                }
                catch (Exception)
                {
                    throw new InternalServerException();
                }
            }

            string code = GenerateConfirmationCode();
            var expirationDate = DateTime.UtcNow.AddMinutes(_authSettings.CodeDurationInMinutes);

            authUser.EmailConfirmationCode = code;
            authUser.EmailConfirmationCodeExpiryTime = expirationDate;

            var updateResult = await _userManager.UpdateAsync(authUser);
            if (!updateResult.Succeeded)
            {
                throw new InternalServerException();
            }

            await SendConfirmationCodeByEmail(authUser.Email!, code);
        }

        public async Task<ProfilePictureUploadResponse> UploadProfilePicture(ProfilePictureUploadRequest request, string? registrationToken)
        {
            var validationResult = await new ProfilePictureUploadValidator().ValidateAsync(request);

            if (validationResult.Errors.Count != 0)
            {
                throw new BadRequestException("Invalid registration", validationResult);
            }

            if (string.IsNullOrWhiteSpace(registrationToken))
            {
                throw new UnauthorizedAccessException("Token is missing");
            }
            var principal = ExtractUserPrincipalFromToken(registrationToken);

            string? email = principal.FindFirst(ClaimTypes.Email)?.Value;
            if (string.IsNullOrWhiteSpace(email))
            {
                throw new UnauthorizedAccessException("Invalid registration token");
            }

            var authUser = await _userManager.FindByEmailAsync(email);

            if (authUser == null)
            {
                throw new NotFoundException($"The user ('{email}') wasn't found.");
            }
            if (authUser.EmailConfirmed)
            {
                throw new InternalServerException();
            }

            var user = await _userRepository.GetByIdAsync(new UserId(authUser.Id));

            var profileImageBlobName = await _profileImageStorageService.UploadProfileImageAsync(request.File.OpenReadStream(), request.File.FileName, new UserId(authUser.Id), user?.ProfileImagePath);

            if (user == null)
            {
                var newUser = new User()
                {
                    Id = new UserId(authUser.Id),
                    Email = authUser.Email ?? "",
                    UserName = authUser.UserName ?? "",
                    ProfileImagePath = profileImageBlobName
                };

                try
                {
                    await _userRepository.CreateAsync(newUser);
                }
                catch (Exception)
                {
                    throw new InternalServerException();
                }
            }
            else
            {
                user.ProfileImagePath = profileImageBlobName;

                try
                {
                    await _userRepository.UpdateAsync(user);
                }
                catch (Exception)
                {
                    throw new InternalServerException();
                }
            }

            return new ProfilePictureUploadResponse()
            {
                ProfilePictureUrl = _profileImageStorageService.GetProfileImageUrl(profileImageBlobName)
            };
        }

        public async Task RegistrationStep3(RegistrationStep3Request request, string? registrationToken)
        {
            var validationResult = await new RegistrationStep3Validator().ValidateAsync(request);

            if (validationResult.Errors.Count != 0)
            {
                throw new BadRequestException("Invalid registration", validationResult);
            }

            if (string.IsNullOrWhiteSpace(registrationToken))
            {
                throw new UnauthorizedAccessException("Token is missing");
            }
            var principal = ExtractUserPrincipalFromToken(registrationToken);

            string? email = principal.FindFirst(ClaimTypes.Email)?.Value;
            if (string.IsNullOrWhiteSpace(email))
            {
                throw new UnauthorizedAccessException("Invalid registration token");
            }

            var authUser = await _userManager.FindByEmailAsync(email);

            if (authUser == null)
            {
                throw new NotFoundException($"The user ('{email}') wasn't found.");
            }
            if (authUser.EmailConfirmed)
            {
                throw new InternalServerException();
            }

            if (authUser.EmailConfirmationCodeExpiryTime <= DateTime.UtcNow || authUser.EmailConfirmationCode == null)
            {
                var expiredCodeValidationResult = new ValidationResult(new List<ValidationFailure>
                {
                    new ValidationFailure("Code", "The code is expired. Try again.")
                });

                throw new BadRequestException("Invalid registration", expiredCodeValidationResult);
            }

            if (request.Code == authUser.EmailConfirmationCode)
            {
                authUser.EmailConfirmed = true;
                authUser.EmailConfirmationCode = null;
                authUser.EmailConfirmationCodeExpiryTime = DateTime.MinValue;
                var updateResult = await _userManager.UpdateAsync(authUser);
                if (!updateResult.Succeeded)
                {
                    throw new InternalServerException();
                }
            }
            else
            {
                var incorrectCodeValidationResult = new ValidationResult(new List<ValidationFailure>
                {
                    new ValidationFailure("Code", "The code is incorrect.")
                });

                throw new BadRequestException("Invalid registration", incorrectCodeValidationResult);
            }
        }

        public async Task<CompleteAuthResponse> RefreshToken(RefreshTokenRequest request, string? refreshToken)
        {
            var validationResult = await new RefreshValidator().ValidateAsync(request);

            if (validationResult.Errors.Count != 0)
            {
                throw new BadRequestException("Invalid refresh request.", validationResult);
            }

            if (string.IsNullOrWhiteSpace(refreshToken))
            {
                throw new UnauthorizedAccessException("Refresh token is missing.");
            }

            string accessToken = request.AccessToken;

            var principal = ExtractPrincipalFromExpiredToken(accessToken);
            string? email = principal.FindFirst(ClaimTypes.Email)?.Value;
            string? deviceId = principal.FindFirst("device_id")?.Value;
            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(deviceId))
            {
                throw new UnauthorizedAccessException("Invalid access token.");
            }

            var authUser = await _userManager.FindByEmailAsync(email);

            if (authUser == null)
            {
                throw new UnauthorizedAccessException($"The user ('{email}') wasn't found.");
            }

            var existingRefreshToken = await _context.RefreshTokens
                    .FirstOrDefaultAsync(rt => rt.DeviceId == deviceId && rt.UserId == authUser.Id);

            if (existingRefreshToken == null)
            {
                throw new UnauthorizedAccessException("Invalid access token or refresh token.");
            }

            if (existingRefreshToken.Token != refreshToken || existingRefreshToken.ExpiryTime <= DateTime.UtcNow)
            {
                throw new UnauthorizedAccessException("Refresh token is invalid or expired.");
            }


            return await GenerateAuthResponse(authUser, deviceId);
        }

        public async Task ForgotPassword(ForgotPasswordRequest request)
        {
            var validationResult = await new ForgotPasswordVaidator().ValidateAsync(request);

            if (validationResult.Errors.Count != 0)
            {
                throw new BadRequestException("Invalid forgot password request", validationResult);
            }

            var authUser = await _userManager.FindByEmailAsync(request.Email);

            if (authUser == null)
            {
                throw new NotFoundException($"The user ({request.Email}) was not found.");
            }
            if (authUser.EmailConfirmed == false)
            {
                throw new ForbiddenException("The email is not confirmed. Finish your registration");
            }
            if (string.IsNullOrWhiteSpace(authUser.PasswordHash))
            {
                var passwordValidationResult = new ValidationResult(new List<ValidationFailure>
                {
                    new ValidationFailure("Email", "The account was registered with an external provider. Try another way")
                });

                throw new BadRequestException("Invalid forgot password request", passwordValidationResult);
            }

            var token = await _userManager.GeneratePasswordResetTokenAsync(authUser);

            var param = new Dictionary<string, string?>()
            {
                { "token", token },
                { "email", request.Email }
            };

            var callback = QueryHelpers.AddQueryString(request.ClientUri, param);
            await SendPasswordResetLinkByEmail(authUser.Email!, callback);
        }

        public async Task ResetPassword(ResetPasswordRequest request)
        {
            var validationResult = await new ResetPasswordValidator().ValidateAsync(request);

            if (validationResult.Errors.Count != 0)
            {
                throw new BadRequestException("Invalid reset password request", validationResult);
            }

            var authUser = await _userManager.FindByEmailAsync(request.Email);

            if (authUser == null)
            {
                throw new NotFoundException($"The user ({request.Email}) was not found.");
            }

            var decodedToken = Uri.UnescapeDataString(request.Token);

            var result = await _userManager.ResetPasswordAsync(authUser, decodedToken, request.Password);

            if (!result.Succeeded)
            {
                var resultValidationFailures = result.Errors
                    .Select(e => new ValidationFailure("ResetPassword", e.Description))
                    .ToList();

                var resultValidationResult = new ValidationResult(resultValidationFailures);

                throw new BadRequestException("Reset password failed", resultValidationResult);
            }

        }

        public async Task Logout(ClaimsPrincipal userPrincipal)
        {
            string? email = userPrincipal.FindFirst(ClaimTypes.Email)?.Value;
            string? deviceId = userPrincipal.FindFirst("device_id")?.Value;

            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(deviceId))
            {
                throw new UnauthorizedAccessException("Invalid access token.");
            }

            var authUser = await _userManager.FindByEmailAsync(email);
            if (authUser == null)
            {
                throw new NotFoundException($"The user ('{email}') wasn't found.");
            }

            var existingRefreshToken = await _context.RefreshTokens
                    .FirstOrDefaultAsync(rt => rt.DeviceId == deviceId && rt.UserId == authUser.Id);

            if (existingRefreshToken == null)
            {
                throw new UnauthorizedAccessException("Invalid access token or refresh token.");
            }

            try
            {
                _context.RefreshTokens.Remove(existingRefreshToken);
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw new InternalServerException();
            }


        }

        private async Task<CompleteAuthResponse> GenerateAuthResponse(AuthUser authUser, string deviceId)
        {
            var user = await _userRepository.GetByIdAsync(new UserId(authUser.Id));

            if (user == null)
            {
                throw new InternalServerException();
            }

            var accessToken = await GenerateJwtToken(authUser, _jwtSettings.AccessTokenValidityInMinutes, deviceId);
            var refreshToken = await GenerateRefreshToken(authUser, deviceId);


            var authResponse = new AuthResponse
            {
                Id = authUser.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                UserName = authUser.UserName ?? "",
                Email = authUser.Email ?? "",
                ProfileImageUrl = _profileImageStorageService.GetProfileImageUrl(user.ProfileImagePath ?? _defaultFiles.DefaultProfilePicture),
                Roles = await _userManager.GetRolesAsync(authUser),
                Token = accessToken
            };
            var refreshTokenResponse = new RefreshTokenResponse
            {
                Token = refreshToken.Token,
                ExpiryTime = refreshToken.ExpiryTime
            };

            return new CompleteAuthResponse
            {
                AuthResponse = authResponse,
                RefreshTokenResponse = refreshTokenResponse
            };
        }

        private async Task<string> GenerateJwtToken(AuthUser user, double durationInMinutes, string deviceId = "")
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = _jwtSettings.Key;
            if (string.IsNullOrWhiteSpace(key))
            {
                throw new InvalidOperationException("JWT Key is missing from configuration.");
            }
            if (string.IsNullOrWhiteSpace(user.Email) || string.IsNullOrWhiteSpace(user.UserName))
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
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim("device_id", deviceId)
            }
            .Union(userClaims)
            .Union(roleClaims);

            var keyBytes = Encoding.UTF8.GetBytes(key);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(authClaims),
                Expires = DateTime.UtcNow.AddMinutes(durationInMinutes),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(keyBytes), SecurityAlgorithms.HmacSha256),
                Issuer = _jwtSettings.Issuer,
                Audience = _jwtSettings.Audience
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        private async Task<RefreshToken> GenerateRefreshToken(AuthUser user, string deviceId)
        {
            var existingRefreshToken = await _context.RefreshTokens
                    .FirstOrDefaultAsync(rt => rt.DeviceId == deviceId && rt.UserId == user.Id);

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
                    await _context.RefreshTokens.AddAsync(refreshToken);
                    await _context.SaveChangesAsync();
                    return refreshToken;
                }
            }
            catch (Exception)
            {
                throw new InternalServerException();
            }
        }

        private ClaimsPrincipal ExtractUserPrincipalFromToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_jwtSettings.Key);

            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = _jwtSettings.Issuer,
                ValidAudience = _jwtSettings.Audience,
                IssuerSigningKey = new SymmetricSecurityKey(key)
            };

            try
            {
                var principal = tokenHandler.ValidateToken(token, validationParameters, out var validatedToken);
                var jwtSecurityToken = validatedToken as JwtSecurityToken;
                if (jwtSecurityToken == null || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                {
                    throw new UnauthorizedAccessException("Token validation failed");
                }
                return principal;
            }
            catch (Exception)
            {
                throw new UnauthorizedAccessException("Token validation failed");
            }
        }

        private ClaimsPrincipal ExtractPrincipalFromExpiredToken(string token)
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
                var principal = tokenHandler.ValidateToken(token, validationParameters, out var validatedToken);
                var jwtSecurityToken = validatedToken as JwtSecurityToken;
                if (jwtSecurityToken == null || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                {
                    throw new UnauthorizedAccessException("Token validation failed");
                }
                return principal;
            }
            catch
            {
                throw new UnauthorizedAccessException("Token validation failed");
            }
        }

        private async Task<string> DownloadAndUploadProfileImageFromUrlAsync(string imageUrl, string fileName, UserId userId)
        {
            using var httpClient = new HttpClient();
            using var response = await httpClient.GetAsync(imageUrl);

            if (!response.IsSuccessStatusCode)
            {
                throw new BadRequestException("Unable to download image from external provider.");
            }

            await using var imageStream = await response.Content.ReadAsStreamAsync();
            return await _profileImageStorageService.UploadProfileImageAsync(imageStream, fileName, userId);
        }

        private async Task TrySetExternalProfileImageAsync(UserId userId, string? imageUrl, string fileName)
        {
            if (string.IsNullOrWhiteSpace(imageUrl))
                return;

            try
            {
                var uploadedPath = await DownloadAndUploadProfileImageFromUrlAsync(imageUrl, fileName, userId);
                await _userRepository.UpdateProfileImageAsync(userId, uploadedPath);
            }
            catch
            { }
        }

        private string GenerateConfirmationCode()
        {
            var random = new Random();
            return random.Next(100000, 999999).ToString();
        }

        private async Task SendConfirmationCodeByEmail(string toEmail, string confirmationCode)
        {
            string subject = "Email Confirmation Code";

            string htmlBody = $@"
            <html>
                <body style='font-family: Arial, sans-serif;'>
                    <div style='max-width: 600px; margin: auto; padding: 20px; border: 1px solid #ccc;'>
                        <h2 style='color: #333;'>Confirm Your Email Address</h2>
                        <p style='font-size: 16px; color: #555;'>Thank you for registering! Please use the following code to complete your registration:</p>
                        <div style='padding: 15px; background-color: #f7f7f7; text-align: center; border-radius: 5px;'>
                            <h3 style='color: #444;'>{confirmationCode}</h3>
                        </div>
                        <p style='font-size: 16px; color: #555;'>If you didn't request this, please ignore this email.</p>
                        <p style='font-size: 14px; color: #aaa;'>Best regards,<br>Your Glowee Team</p>
                    </div>
                </body>
            </html>";

            await _emailSender.SendEmailAsync(new EmailMessage()
            {
                To = toEmail,
                Body = htmlBody,
                Subject = subject,
                IsBodyHtml = true
            });
        }

        private async Task SendPasswordResetLinkByEmail(string toEmail, string resetLink)
        {
            string subject = "Password Reset Request";

            string htmlBody = $@"
            <html>
                <body style='font-family: Arial, sans-serif;'>
                    <div style='max-width: 600px; margin: auto; padding: 20px; border: 1px solid #ccc;'>
                        <h2 style='color: #333;'>Reset Your Password</h2>
                        <p style='font-size: 16px; color: #555;'>We received a request to reset your password. Click the link below to choose a new password:</p>
                        <div style='margin: 20px 0; text-align: center;'>
                            <a href='{resetLink}' style='display: inline-block; padding: 12px 24px; background-color: #007bff; color: #fff; text-decoration: none; border-radius: 5px; font-size: 16px;'>Reset Password</a>
                        </div>
                        <p style='font-size: 16px; color: #555;'>If you didn’t request a password reset, you can safely ignore this email.</p>
                        <p style='font-size: 14px; color: #aaa;'>Best regards,<br>Your Glowee Team</p>
                    </div>
                </body>
            </html>";

            await _emailSender.SendEmailAsync(new EmailMessage()
            {
                To = toEmail,
                Body = htmlBody,
                Subject = subject,
                IsBodyHtml = true
            });
        }
    }
}
