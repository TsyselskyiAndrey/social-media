using Glowee.Application.Contracts.Identity;
using Glowee.Application.Exceptions;
using Glowee.Application.Models.Identity.GoogleAuth;
using Glowee.Application.Models.Identity.Settings;
using Google.Apis.Auth;
using Microsoft.Extensions.Options;

namespace Glowee.Identity.Services
{
    public class GoogleAuthService : IGoogleAuthService
    {
        private readonly GoogleSettings _googleSettings;
        public GoogleAuthService(IOptions<GoogleSettings> googleSettings)
        {
            _googleSettings = googleSettings.Value;
        }
        public async Task<GoogleJsonWebSignature.Payload> VerifyGoogleToken(GoogleAuthRequest authRequest)
        {
            try
            {
                var settings = new GoogleJsonWebSignature.ValidationSettings()
                {
                    Audience = new List<string>() { _googleSettings.WebClientId, _googleSettings.AndroidClientId, _googleSettings.IOSClientId }
                };
                var payload = await GoogleJsonWebSignature.ValidateAsync(authRequest.IdToken, settings);
                return payload;
            }
            catch (Exception)
            {
                throw new BadRequestException("IdToken is invalid.");
            }
        }
    }
}
