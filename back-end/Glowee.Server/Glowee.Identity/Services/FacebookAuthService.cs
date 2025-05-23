using Glowee.Application.Contracts.Identity;
using Glowee.Application.Exceptions;
using Glowee.Application.Models.Identity.FacebookAuth;
using Glowee.Application.Models.Identity.Settings;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace Glowee.Identity.Services
{
    public class FacebookAuthService : IFacebookAuthService
    {
        private readonly FacebookSettings _facebookSettings;
        private readonly HttpClient _httpClient;

        public FacebookAuthService(IOptions<FacebookSettings> facebookSettings, IHttpClientFactory httpClientFactory)
        {
            _facebookSettings = facebookSettings.Value;
            _httpClient = httpClientFactory.CreateClient("Facebook");
        }

        public async Task<FacebookTokenValidationResponse> VerifyFacebookToken(string accessToken)
        {
            try
            {
                string TokenValidationUrl = _facebookSettings.TokenValidationUrl;
                var url = string.Format(TokenValidationUrl, accessToken, _facebookSettings.AppId, _facebookSettings.AppSecret);
                var response = await _httpClient.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    var responseAsString = await response.Content.ReadAsStringAsync();

                    var tokenValidationResponse = JsonConvert.DeserializeObject<FacebookTokenValidationResponse>(responseAsString);

                    if (tokenValidationResponse == null)
                    {
                        throw new InternalServerException();
                    }

                    return tokenValidationResponse;
                }
                throw new BadRequestException("Access token is invalid.");
            }
            catch (Exception)
            {
                throw new BadRequestException("Access token is invalid.");
            }
        }
        public async Task<FacebookUserInfoResponse> GetUserInfoAsync(string accessToken)
        {
            try
            {
                string userInfoUrl = _facebookSettings.UserInfoUrl;
                string url = string.Format(userInfoUrl, accessToken);

                var response = await _httpClient.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    var responseAsString = await response.Content.ReadAsStringAsync();
                    var userInfoResponse = JsonConvert.DeserializeObject<FacebookUserInfoResponse>(responseAsString);

                    if (userInfoResponse == null)
                    {
                        throw new InternalServerException();
                    }

                    return userInfoResponse;
                }
                throw new BadRequestException("Access token is invalid.");
            }
            catch (Exception)
            {
                throw new BadRequestException("Access token is invalid.");
            }
        }
    }
}
