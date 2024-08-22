using MediatR;
using Microsoft.AspNetCore.Identity;
using Mini_ECommerce.Application.Abstractions.Services.Auth;
using Mini_ECommerce.Application.Abstractions.Services.Token;
using Mini_ECommerce.Application.DTOs.Token;
using Mini_ECommerce.Application.DTOs.User;
using Mini_ECommerce.Application.Exceptions;
using Mini_ECommerce.Domain.Entities.Identity;
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
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IAppTokenHandler _tokenHandler;

        public InternalAuthService(UserManager<AppUser> userManager, IAppTokenHandler tokenHandler, SignInManager<AppUser> signInManager)
        {
            _userManager = userManager;
            _tokenHandler = tokenHandler;
            _signInManager = signInManager;
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

            return new()
            {
                Token = token,
                Message = "User login successfully!"
            };
        }

        public Task<TokenDTO> RefreshTokenLoginAsync(string refreshToken)
        {
            throw new NotImplementedException();
        }
    }
}
