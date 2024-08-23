using MediatR;
using Microsoft.AspNetCore.Identity;
using Mini_ECommerce.Application.Abstractions.Services;
using Mini_ECommerce.Application.Abstractions.Services.Token;
using Mini_ECommerce.Application.DTOs.User;
using Mini_ECommerce.Application.Exceptions;
using Mini_ECommerce.Application.Features.Commands.AppUser.RegisterUser;
using Mini_ECommerce.Domain.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mini_ECommerce.Persistence.Concretes.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IAppTokenHandler _tokenHandler;

        public UserService(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, IAppTokenHandler tokenHandler)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _tokenHandler = tokenHandler;
        }

        public async Task<RegisterUserResponseDTO> RegisterUserAsync(RegisterUserRequestDTO userRequestDTO)
        {
            AppUser user = new() { FirstName = userRequestDTO.FirstName, LastName = userRequestDTO.LastName, UserName = userRequestDTO.UserName, Email = userRequestDTO.Email };

            var result = await _userManager.CreateAsync(user, userRequestDTO.Password);

            if (!result.Succeeded)
            {
                string message = string.Empty;

                foreach (var error in result.Errors)
                {
                    message += $"{error.Code} - {error.Description}\n";
                }

                throw new RegistrationException(message);
            }

            await _signInManager.SignInAsync(user, isPersistent: false);

            var token = _tokenHandler.CreateAccessToken(10000, user);
            await UpdateRefreshTokenAsync(token.RefreshToken, user, token.ExpirationDate, 15); // (minutes)

            return new RegisterUserResponseDTO()
            {
                Success = true,
                Message = "User created successfully",
                Token = token
            };
        }

        public async Task UpdateRefreshTokenAsync(string refreshToken, AppUser user, DateTime accessTokenLifeTime, int addOnAccessTokenLifeTime)
        {
            if (user != null)
            {
                user.RefreshToken = refreshToken;
                user.RefreshTokenEndDate = accessTokenLifeTime.AddMinutes(addOnAccessTokenLifeTime);

                await _userManager.UpdateAsync(user);
            }
            else
                throw new EntityNotFoundException(nameof(user), "User not found to update refresh token");
        }
    }
}
