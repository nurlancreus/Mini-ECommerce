using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Mini_ECommerce.Application.Abstractions.Services;
using Mini_ECommerce.Application.Abstractions.Services.Auth;
using Mini_ECommerce.Application.Abstractions.Services.Token;
using Mini_ECommerce.Application.DTOs.Token;
using Mini_ECommerce.Application.DTOs.User;
using Mini_ECommerce.Application.Exceptions;
using Mini_ECommerce.Domain.Entities.Identity;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Mini_ECommerce.Persistence.Concretes.Services.Auth
{
    public class InternalAuthService : IInternalAuthService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IUserService _userService;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IAppTokenHandler _tokenHandler;

        public InternalAuthService(UserManager<AppUser> userManager, IUserService userService, SignInManager<AppUser> signInManager, IAppTokenHandler tokenHandler)
        {
            _userManager = userManager;
            _userService = userService;
            _signInManager = signInManager;
            _tokenHandler = tokenHandler;
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
    }
}
