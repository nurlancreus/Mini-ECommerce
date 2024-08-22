using MediatR;
using Microsoft.AspNetCore.Identity;
using Mini_ECommerce.Application.Abstractions.Services;
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

        public UserService(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<RegisterUserResponseDTO> RegisterUserAsync(RegisterUserRequestDTO userRequestDTO)
        {
            var result = await _userManager.CreateAsync(new() { FirstName = userRequestDTO.FirstName, LastName = userRequestDTO.LastName, UserName = userRequestDTO.UserName, Email = userRequestDTO.Email }, userRequestDTO.Password);

            if (!result.Succeeded)
            {
                string message = string.Empty;

                foreach (var error in result.Errors)
                {
                    message += $"{error.Code} - {error.Description}\n";
                }

                throw new RegistrationException(message);
            }

            return new RegisterUserResponseDTO()
            {
                Success = true,
                Message = "User created successfully"
            };
        }
    }
}
