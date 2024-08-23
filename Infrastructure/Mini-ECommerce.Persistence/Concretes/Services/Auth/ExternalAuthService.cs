using Google.Apis.Auth;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Mini_ECommerce.Application.Abstractions.Services;
using Mini_ECommerce.Application.Abstractions.Services.Auth;
using Mini_ECommerce.Application.Abstractions.Services.Token;
using Mini_ECommerce.Application.DTOs.FacebookAccess;
using Mini_ECommerce.Application.DTOs.Token;
using Mini_ECommerce.Application.DTOs.User;
using Mini_ECommerce.Application.Exceptions;
using Mini_ECommerce.Application.Features.Commands.AppUser.FacebookLoginUser;
using Mini_ECommerce.Domain.Entities.Identity;
using Mini_ECommerce.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Mini_ECommerce.Persistence.Concretes.Services.Auth
{
    public class ExternalAuthService : AuthService, IExternalAuthService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly HttpClient _httpClient;
        private readonly IUserService _userService;
        private readonly IAppTokenHandler _tokenHandler;
        private readonly IConfiguration _configuration;

        public ExternalAuthService(UserManager<AppUser> userManager, IHttpClientFactory httpClientFactory, IAppTokenHandler tokenHandler, IConfiguration configuration, IUserService userService)
        {
            _userManager = userManager;
            _httpClient = httpClientFactory.CreateClient();
            _tokenHandler = tokenHandler;
            _configuration = configuration;
            _userService = userService;
        }

        public async Task<TokenDTO> FacebookLoginAsync(string authToken, string provider, int accessTokenLifeTime)
        {
            string accessTokenResponse = await _httpClient.GetStringAsync($"https://graph.facebook.com/oauth/access_token?client_id={_configuration["ExternalLoginSettings:Facebook:AppId"]}&client_secret={_configuration["ExternalLoginSettings:Facebook:AppId"]}&grant_type=client_credentials");

            var facebookAccessToken = JsonSerializer.Deserialize<FacebookAccessTokenResponseDTO>(accessTokenResponse);

            string userAccessTokenValidation = await _httpClient.GetStringAsync($"https://graph.facebook.com/debug_token?input_token={authToken}&access_token={facebookAccessToken}");

            var validation = JsonSerializer.Deserialize<FacebookUserAccessTokenValidationDTO>(userAccessTokenValidation);

            if (validation?.Data.IsValid == null)
            {
                throw new LoginException("Invalid external authentication.");

            }
            string userInfoResponse = await _httpClient.GetStringAsync($"https://graph.facebook.com/me?fields=email,first_name,last_name&access_token={authToken}");

            var facebookUserInfo = JsonSerializer.Deserialize<FacebookUserInfoResponseDTO>(userInfoResponse);

            var info = new UserLoginInfo(provider, validation.Data.UserId, provider);

            var token = await RegisterExternalUserAsync(new ()
            {
                FirstName = facebookUserInfo.FirstName,
                LastName = facebookUserInfo.LastName,
                Email = facebookUserInfo.Email,
                UserName = facebookUserInfo.Email,
                UserInfo = info,
                
            }, accessTokenLifeTime);

            return token;
        }

        public async Task<TokenDTO> GoogleLoginAsync(string idToken, string provider, int accessTokenLifeTime)
        {
            var settings = new GoogleJsonWebSignature.ValidationSettings()
            {
                Audience = [_configuration["ExternalLoginSettings:Google:ClientId"]]
            };

            var payload = await GoogleJsonWebSignature.ValidateAsync(idToken, settings);

            var info = new UserLoginInfo(provider, payload.Subject, provider);

            var token = await RegisterExternalUserAsync(new()
            {
                FirstName = payload.GivenName,
                LastName = payload.FamilyName,
                Email = payload.Email,
                UserName = payload.Email,
                UserInfo = info,
            }, accessTokenLifeTime);

            return token;
        }

        private async Task<TokenDTO> RegisterExternalUserAsync(RegisterExternalUserDTO registerExternalUserDTO, int accessTokenLifeTime)
        {
            var info = registerExternalUserDTO.UserInfo;

            var user = await _userManager.FindByLoginAsync(info.LoginProvider, info.ProviderKey);

            bool result = user != null;

            if (user == null)
            {
                user = await _userManager.FindByEmailAsync(registerExternalUserDTO.Email);

                if (user == null)
                {
                    user = new()
                    {
                        Email = registerExternalUserDTO.Email,
                        UserName = registerExternalUserDTO.Email,
                        FirstName = registerExternalUserDTO.FirstName,
                        LastName = registerExternalUserDTO.LastName,
                    };

                    var identityResult = await _userManager.CreateAsync(user);
                    result = identityResult.Succeeded;
                }
            }

            if (!result)
            {
                throw new Exception("Invalid external authentication.");
            }

            await _userManager.AddLoginAsync(user, info); //AspNetUserLogins

            TokenDTO token = _tokenHandler.CreateAccessToken(accessTokenLifeTime, user);

            await _userService.UpdateRefreshTokenAsync(token.RefreshToken, user, token.ExpirationDate, 15); // (minutes)

            return token;
        }
    }
}
