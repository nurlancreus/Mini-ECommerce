using Google.Apis.Auth;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Mini_ECommerce.Application.Abstractions.Services;
using Mini_ECommerce.Application.Abstractions.Services.Auth;
using Mini_ECommerce.Application.Abstractions.Services.Token;
using Mini_ECommerce.Application.DTOs.FacebookAccess;
using Mini_ECommerce.Application.DTOs.Token;
using Mini_ECommerce.Application.DTOs.User;
using Mini_ECommerce.Application.Exceptions;
using Mini_ECommerce.Application.Helpers;
using Mini_ECommerce.Domain.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Mini_ECommerce.Persistence.Concretes.Services.Auth
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IMailService _mailService;
        private readonly HttpClient _httpClient;
        private readonly IUserService _userService;
        private readonly IAppTokenHandler _tokenHandler;
        private readonly IConfiguration _configuration;
        private readonly SignInManager<AppUser> _signInManager;

        public AuthService(UserManager<AppUser> userManager, IMailService mailService, IHttpClientFactory httpClientFactory, IUserService userService, IAppTokenHandler tokenHandler, IConfiguration configuration, SignInManager<AppUser> signInManager)
        {
            _userManager = userManager;
            _mailService = mailService;
            _httpClient = httpClientFactory.CreateClient();
            _userService = userService;
            _tokenHandler = tokenHandler;
            _configuration = configuration;
            _signInManager = signInManager;
        }

        public async Task ResetPasswordAsync(string email)
        {
            AppUser? user = await _userManager.FindByEmailAsync(email);
            if (user != null)
            {
                string resetToken = await _userManager.GeneratePasswordResetTokenAsync(user);

                //byte[] tokenBytes = Encoding.UTF8.GetBytes(resetToken);
                //resetToken = WebEncoders.Base64UrlEncode(tokenBytes);
                resetToken = resetToken.UrlEncode();

                await _mailService.SendPasswordResetMailAsync(user.Email!, user.Id, resetToken);
            }
        }

        public async Task<bool> VerifyResetTokenAsync(string resetToken, string userId)
        {
            AppUser? user = await _userManager.FindByIdAsync(userId);
            if (user != null)
            {
                //byte[] tokenBytes = WebEncoders.Base64UrlDecode(resetToken);
                //resetToken = Encoding.UTF8.GetString(tokenBytes);
                resetToken = resetToken.UrlDecode();

                return await _userManager.VerifyUserTokenAsync(user, _userManager.Options.Tokens.PasswordResetTokenProvider, "ResetPassword", resetToken);
            }
            return false;
        }

        public async Task<LoginUserResponseDTO> LoginAsync(LoginUserRequestDTO loginUserRequestDTO)
        {
            var user = await _userManager.FindByEmailAsync(loginUserRequestDTO.Email)
            ?? throw new LoginException("Invalid login attempt.");
            var signInResult = await _signInManager.PasswordSignInAsync(user.UserName!, loginUserRequestDTO.Password, loginUserRequestDTO.RememberMe, lockoutOnFailure: false);

            if (!signInResult.Succeeded)
            {
                throw new LoginException("Invalid login attempt.");
            }

            var token = _tokenHandler.CreateAccessToken(user);
            await _userService.UpdateRefreshTokenAsync(token.RefreshToken, user, token.ExpirationDate);

            return new()
            {
                Token = token,
                Message = "User login successfully!"
            };
        }

        public async Task<TokenDTO> RefreshTokenLoginAsync(string accessToken, string refreshToken)
        {

            ClaimsPrincipal? principal = _tokenHandler.GetPrincipalFromAccessToken(accessToken) ?? throw new Exception("Invalid jwt access token");

            string? name = principal.FindFirstValue(ClaimTypes.Name);

            AppUser? user = await _userManager.FindByNameAsync(name);

            if (user == null || user.RefreshToken != refreshToken || user.RefreshTokenEndDate <= DateTime.UtcNow)
            {
                throw new Exception("Invalid refresh token");
            }

            var token = _tokenHandler.CreateAccessToken(user);

            await _userService.UpdateRefreshTokenAsync(token.RefreshToken, user, token.ExpirationDate);

            return token;


            /* AppUser? user = await _userManager.Users.FirstOrDefaultAsync(u => u.RefreshToken == refreshToken);

             if (user != null && user?.RefreshTokenEndDate > DateTime.UtcNow)
             {
                 var token = _tokenHandler.CreateAccessToken(user);
                 await _userService.UpdateRefreshTokenAsync(token.RefreshToken, user, token.ExpirationDate);
                 return token;
             }
             else
                 throw new EntityNotFoundException(nameof(user)); */
        }

        public async Task<TokenDTO> FacebookLoginAsync(string authToken, string provider)
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

            var token = await RegisterExternalUserAsync(new()
            {
                FirstName = facebookUserInfo.FirstName,
                LastName = facebookUserInfo.LastName,
                Email = facebookUserInfo.Email,
                UserName = facebookUserInfo.Email,
                UserInfo = info,

            });

            return token;
        }

        public async Task<TokenDTO> GoogleLoginAsync(string idToken, string provider)
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
            });

            return token;
        }

        private async Task<TokenDTO> RegisterExternalUserAsync(RegisterExternalUserDTO registerExternalUserDTO)
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

            TokenDTO token = _tokenHandler.CreateAccessToken(user);

            await _userService.UpdateRefreshTokenAsync(token.RefreshToken, user, token.ExpirationDate);

            return token;
        }
    }
}
