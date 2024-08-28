using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Mini_ECommerce.Application.Abstractions.Services;
using Mini_ECommerce.Application.Abstractions.Services.Token;
using Mini_ECommerce.Application.DTOs.User;
using Mini_ECommerce.Application.Exceptions;
using Mini_ECommerce.Application.Extensions;
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
        private readonly IConfiguration _configuration;

        public int TotalUsersCount => throw new NotImplementedException();

        public UserService(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, IAppTokenHandler tokenHandler, IConfiguration configuration)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _tokenHandler = tokenHandler;
            _configuration = configuration;
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

            var token = _tokenHandler.CreateAccessToken(user);
            await UpdateRefreshTokenAsync(token.RefreshToken, user, token.ExpirationDate);

            return new RegisterUserResponseDTO()
            {
                Success = true,
                Message = "User created successfully",
                Token = token
            };
        }

        public async Task UpdateRefreshTokenAsync(string refreshToken, AppUser user, DateTime accessTokenLifeTime)
        {
            if (user != null)
            {
                double refreshTokenLifeTime = Convert.ToDouble(_configuration["Token:RefreshTokenLifeTimeInMinutes"]);

                user.RefreshToken = refreshToken;
                user.RefreshTokenEndDate = accessTokenLifeTime.AddMinutes(refreshTokenLifeTime);

                await _userManager.UpdateAsync(user);
            }
            else
                throw new EntityNotFoundException(nameof(user), "User not found to update refresh token");
        }

        public async Task UpdatePasswordAsync(string userId, string resetToken, string newPassword)
        {
            AppUser? user = await _userManager.FindByIdAsync(userId);

            if (user != null)
            {
                resetToken = resetToken.UrlDecode();
                IdentityResult result = await _userManager.ResetPasswordAsync(user, resetToken, newPassword);

                if (result.Succeeded)
                    await _userManager.UpdateSecurityStampAsync(user); // reset 'reset' token
                else
                    throw new PasswordChangeFailedException();
            }
        }

        public Task<List<GetAppUserDTO>> GetAllUsersAsync(int page, int size)
        {
            throw new NotImplementedException();
        }

        public Task AssignRoleToUserAsnyc(string userId, string[] roles)
        {
            throw new NotImplementedException();
        }

        public Task<string[]> GetRolesToUserAsync(string userIdOrName)
        {
            throw new NotImplementedException();
        }

        public Task<bool> HasRolePermissionToEndpointAsync(string name, string code)
        {
            throw new NotImplementedException();
        }
    }
}
