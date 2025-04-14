using Glowee.Application.Contracts.Identity;
using Glowee.Application.Models.Identity.FacebookAuth;
using Glowee.Application.Models.Identity.GoogleAuth;
using Glowee.Application.Models.Identity.LogIn;
using Glowee.Application.Models.Identity.RefreshToken;
using Glowee.Application.Models.Identity.Registration;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Glowee.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("facebook-login")]
        public async Task<IActionResult> FacebookLogin(FacebookAuthRequest facebookRequest)
        {
            var completeAuthResponse = await _authService.FacebookLogin(facebookRequest);

            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.None,
                Expires = completeAuthResponse.RefreshTokenResponse.ExpiryTime
            };
            Response.Cookies.Append("refreshToken", completeAuthResponse.RefreshTokenResponse.Token, cookieOptions);

            return Ok(completeAuthResponse.AuthResponse);
        }

        [HttpPost("google-login")]
        public async Task<IActionResult> GoogleLogin(GoogleAuthRequest googleRequest)
        {
            var completeAuthResponse = await _authService.GoogleLogin(googleRequest);

            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.None,
                Expires = completeAuthResponse.RefreshTokenResponse.ExpiryTime
            };
            Response.Cookies.Append("refreshToken", completeAuthResponse.RefreshTokenResponse.Token, cookieOptions);

            return Ok(completeAuthResponse.AuthResponse);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LogInRequest logInRequest)
        {
            var completeAuthResponse = await _authService.Login(logInRequest);

            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.None,
                Expires = completeAuthResponse.RefreshTokenResponse.ExpiryTime
            };
            Response.Cookies.Append("refreshToken", completeAuthResponse.RefreshTokenResponse.Token, cookieOptions);

            return Ok(completeAuthResponse.AuthResponse);
        }

        [HttpPost("registration-step-1")]
        public async Task<IActionResult> RegistrationStep1(RegistrationStep1Request registrationStep1Request)
        {
            var registrationStep1Response = await _authService.RegistrationStep1(registrationStep1Request);

            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.None,
                Expires = registrationStep1Response.ExpiryTime
            };
            Response.Cookies.Append("registrationToken", registrationStep1Response.Token, cookieOptions);

            return Ok();
        }

        [HttpPost("registration-step-2")]
        public async Task<IActionResult> RegistrationStep2(RegistrationStep2Request registrationStep2Request)
        {
            var registrationToken = Request.Cookies["registrationToken"];
            await _authService.RegistrationStep2(registrationStep2Request, registrationToken);
            return Ok();
        }

        [HttpPost("upload-profile-image")]
        public async Task<IActionResult> UploadProfileImage(ProfilePictureUploadRequest profilePictureUploadRequest)
        {
            var registrationToken = Request.Cookies["registrationToken"];
            var profilePictureUrl = await _authService.UploadProfilePicture(profilePictureUploadRequest, registrationToken);
            return Ok(profilePictureUrl);
        }

        [HttpPost("registration-step-3")]
        public async Task<IActionResult> RegistrationStep3(RegistrationStep3Request registrationStep3Request)
        {
            var registrationToken = Request.Cookies["registrationToken"];
            await _authService.RegistrationStep3(registrationStep3Request, registrationToken);

            Response.Cookies.Delete("registrationToken", new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.None
            });

            return Ok();
        }

        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken(RefreshTokenRequest refreshTokenRequest)
        {
            var refreshToken = Request.Cookies["refreshToken"];
            var completeAuthResponse = await _authService.RefreshToken(refreshTokenRequest, refreshToken);

            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.None,
                Expires = completeAuthResponse.RefreshTokenResponse.ExpiryTime
            };
            Response.Cookies.Append("refreshToken", completeAuthResponse.RefreshTokenResponse.Token, cookieOptions);

            return Ok(completeAuthResponse.AuthResponse);
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            await _authService.Logout(User);

            Response.Cookies.Delete("refreshToken", new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.None
            });

            return Ok();
        }
    }
}
