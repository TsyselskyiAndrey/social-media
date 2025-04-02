using Glowee.Application.Contracts.Identity;
using Glowee.Application.Models.Identity.LogIn;
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

        [HttpPost("login")]
        public async Task<IActionResult> Login(LogInRequest logInRequest)
        {
            var logInResponse = await _authService.Login(logInRequest);

            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.None,
                Expires = logInResponse.RefreshTokenResponse.ExpiryTime
            };
            Response.Cookies.Append("refreshToken", logInResponse.RefreshTokenResponse.Token, cookieOptions);

            return Ok(logInResponse.AuthResponse);
        }
    }
}
