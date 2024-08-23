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
using System.Text;
using System.Threading.Tasks;

namespace Mini_ECommerce.Persistence.Concretes.Services.Auth
{
    public class InternalAuthService : AuthService, IInternalAuthService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IUserService _userService;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IAppTokenHandler _tokenHandler;

        public InternalAuthService(UserManager<AppUser> userManager, IAppTokenHandler tokenHandler, SignInManager<AppUser> signInManager, IUserService userService)
        {
            _userManager = userManager;
            _tokenHandler = tokenHandler;
            _signInManager = signInManager;
            _userService = userService;
        }

        public async Task<LoginUserResponseDTO> LoginAsync(LoginUserRequestDTO loginUserRequestDTO, int accessTokenLifeTime)
        {
            var user = await _userManager.FindByEmailAsync(loginUserRequestDTO.Email)
            ?? throw new LoginException("Invalid login attempt.");
            var signInResult = await _signInManager.PasswordSignInAsync(user.UserName!, loginUserRequestDTO.Password, loginUserRequestDTO.RememberMe, lockoutOnFailure: false);

            if (!signInResult.Succeeded)
            {
                throw new LoginException("Invalid login attempt.");
            }

            var token = _tokenHandler.CreateAccessToken(accessTokenLifeTime, user);
            await _userService.UpdateRefreshTokenAsync(token.RefreshToken, user, token.ExpirationDate, 15); // (minutes)

            return new()
            {
                Token = token,
                Message = "User login successfully!"
            };
        }

        public async Task<TokenDTO> RefreshTokenLoginAsync(string refreshToken)
        {
            AppUser? user = await _userManager.Users.FirstOrDefaultAsync(u => u.RefreshToken == refreshToken);

            if (user != null && user?.RefreshTokenEndDate > DateTime.UtcNow)
            {
                var token = _tokenHandler.CreateAccessToken(10000, user);
                await _userService.UpdateRefreshTokenAsync(token.RefreshToken, user, token.ExpirationDate, 15);
                return token;
            }
            else
                throw new EntityNotFoundException(nameof(user));
        }
    }
}
