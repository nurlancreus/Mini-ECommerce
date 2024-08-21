using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Mini_ECommerce.Application.Abstractions.Services.Token;
using Mini_ECommerce.Application.DTOs.FacebookAccess;
using Mini_ECommerce.Application.DTOs.Token;
using Mini_ECommerce.Application.Exceptions;
using Mini_ECommerce.Application.Features.Commands.AppUser.GoogleLoginUser;
using Mini_ECommerce.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Mini_ECommerce.Application.Features.Commands.AppUser.FacebookLoginUser
{
    public class FacebookLoginUserCommandHandler : IRequestHandler<FacebookLoginUserCommandRequest, FacebookLoginUserCommandResponse>
    {
        private readonly UserManager<Domain.Entities.Identity.AppUser> _userManager;
        private readonly HttpClient _httpClient;
        private readonly IAppTokenHandler _tokenHandler;
        private readonly IConfiguration _configuration;

        public FacebookLoginUserCommandHandler(UserManager<Domain.Entities.Identity.AppUser> userManager, IHttpClientFactory httpClientFactory, IConfiguration configuration, IAppTokenHandler tokenHandler)
        {
            _userManager = userManager;
            _httpClient = httpClientFactory.CreateClient();
            _configuration = configuration;
            _tokenHandler = tokenHandler;
        }

        public async Task<FacebookLoginUserCommandResponse> Handle(FacebookLoginUserCommandRequest request, CancellationToken cancellationToken)
        {
            string accessTokenResponse = await _httpClient.GetStringAsync($"https://graph.facebook.com/oauth/access_token?client_id={_configuration["ExternalLoginSettings:Facebook:AppId"]}&client_secret={_configuration["ExternalLoginSettings:Facebook:AppId"]}&grant_type=client_credentials");

            var facebookAccessToken = JsonSerializer.Deserialize<FacebookAccessTokenResponseDTO>(accessTokenResponse);

            string userAccessTokenValidation = await _httpClient.GetStringAsync($"https://graph.facebook.com/debug_token?input_token={request.AuthToken}&access_token={facebookAccessToken}");

            var validation = JsonSerializer.Deserialize<FacebookUserAccessTokenValidationDTO>(userAccessTokenValidation);

            if (validation?.Data.IsValid == null)
            {
                throw new LoginException("Invalid external authentication.");

            }
            string userInfoResponse = await _httpClient.GetStringAsync($"https://graph.facebook.com/me?fields=email,first_name,last_name&access_token={request.AuthToken}");

            var userInfo = JsonSerializer.Deserialize<FacebookUserInfoResponseDTO>(userInfoResponse);

            var info = new UserLoginInfo(ExternalLoginProvider.Facebook.ToString(), validation.Data.UserId, ExternalLoginProvider.Facebook.ToString());

            var user = await _userManager.FindByLoginAsync(info.LoginProvider, info.ProviderKey);

            bool result = user != null;

            if (user == null)
            {
                user = await _userManager.FindByEmailAsync(userInfo.Email);

                if (user == null)
                {
                    user = new()
                    {
                        Email = userInfo.Email,
                        UserName =userInfo.Email,
                        FirstName = userInfo.FirstName,
                        LastName = userInfo.LastName,
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

            TokenDTO token = _tokenHandler.CreateAccessToken(900, user);

            return new FacebookLoginUserCommandResponse()
            {
                Token = new()
                {
                    AccessToken = token.AccessToken,
                }
            };
        }
    }
}
