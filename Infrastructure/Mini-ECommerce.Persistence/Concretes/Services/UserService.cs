using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Mini_ECommerce.Application.Abstractions.Repositories;
using Mini_ECommerce.Application.Abstractions.Services;
using Mini_ECommerce.Application.Abstractions.Services.Token;
using Mini_ECommerce.Application.DTOs.Pagination;
using Mini_ECommerce.Application.DTOs.Role;
using Mini_ECommerce.Application.DTOs.User;
using Mini_ECommerce.Application.Exceptions;
using Mini_ECommerce.Application.Exceptions.Base;
using Mini_ECommerce.Application.Extensions;
using Mini_ECommerce.Application.Features.Commands.User.RegisterUser;
using Mini_ECommerce.Application.Helpers;
using Mini_ECommerce.Domain.Entities.Identity;
using Mini_ECommerce.Domain.Enums;
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
        private readonly RoleManager<AppRole> _roleManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IAppTokenHandler _tokenHandler;
        private readonly IConfiguration _configuration;
        private readonly IPaginationService _paginationService;
        private readonly IAppEndpointReadRepository _appEndpointReadRepository;
        public UserService(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, IAppTokenHandler tokenHandler, IConfiguration configuration, IPaginationService paginationService, RoleManager<AppRole> roleManager, IAppEndpointReadRepository appEndpointReadRepository)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
            _tokenHandler = tokenHandler;
            _configuration = configuration;
            _paginationService = paginationService;
            _appEndpointReadRepository = appEndpointReadRepository;
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

        public async Task<GetAllUsersDTO> GetAllUsersAsync(int page, int size)
        {
            var query = _userManager.Users;

            var paginationRequest = new PaginationRequestDTO()
            {
                Page = page,
                PageSize = size
            };

            var (totalItems, pageSize, currentPage, totalPages, paginatedQuery) = await _paginationService.ConfigurePaginationAsync(paginationRequest, query);

            return new GetAllUsersDTO()
            {
                Users = [.. paginatedQuery.Select(u => new GetAppUserDTO ()
            {
                Id = u.Id,
                FirstName = u.FirstName,
                LastName = u.LastName,
                Email = u.Email!,
                UserName = u.UserName!
            })],
                CurrentPage = currentPage,
                TotalPagesCount = totalPages,
                TotalUserCount = totalItems,
            };
        }

        // Assuming that AppUser is the type for user entities in the application
        public async Task AssignRoleToUserAsync(string userId, string[] roles)
        {
            // Fetch the user by ID
            AppUser? user = await _userManager.FindByIdAsync(userId);

            // Throw an exception if the user does not exist
            if (user == null)
            {
                throw new EntityNotFoundException(nameof(user), userId);
            }

            // Fetch the current roles assigned to the user
            var userRoles = await _userManager.GetRolesAsync(user);

            // Identify roles that need to be removed
            var rolesToDelete = userRoles.Where(role => !roles.Contains(role)).ToList();

            // Remove the roles that are not in the provided roles array
            var removeResult = await _userManager.RemoveFromRolesAsync(user, rolesToDelete);

            // Check if role removal was successful
            if (!removeResult.Succeeded)
            {
                throw new BaseException("Cannot remove already assigned roles", new InvalidDataException());
            }

            // Assign new roles
            foreach (var roleName in roles)
            {
                // Check if the role name is a valid enum value, assuming Role is an enum
                if (!EnumHelpers.TryParseEnum(roleName, out Role parsedRole))
                {
                    throw new InvalidOperationException("Role name cannot be parsed as valid role");
                }

                // Check if the role exists in the role manager
                var role = await _roleManager.FindByNameAsync(roleName);
                if (role == null)
                {
                    throw new EntityNotFoundException(nameof(role));
                }

                // Add the role to the user if not already assigned
                if (!userRoles.Contains(roleName))
                {
                    var addResult = await _userManager.AddToRoleAsync(user, roleName);

                    // Ensure that role addition was successful
                    if (!addResult.Succeeded)
                    {
                        throw new BaseException($"Failed to add role '{roleName}' to user", new InvalidDataException());
                    }
                }
            }

            // Update the user in the database
            var updateResult = await _userManager.UpdateAsync(user);

            // Check if the user update was successful
            if (!updateResult.Succeeded)
            {
                throw new BaseException("Failed to update user after role assignments", new InvalidDataException());
            }
        }

        public async Task<List<GetRoleDTO>> GetRolesAssignedToUserAsync(string userIdOrName)
        {
            // Find the user by ID or username
            AppUser? user = await _userManager.FindByIdAsync(userIdOrName);
            if (user == null)
            {
                user = await _userManager.FindByNameAsync(userIdOrName);
                if (user == null)
                {
                    throw new EntityNotFoundException(nameof(user), userIdOrName);
                }
            }

            // Get the roles assigned to the user
            var userRoles = await _userManager.GetRolesAsync(user);

            var roleDtos = new List<GetRoleDTO>();

            // Iterate over the user roles to fetch role details
            foreach (var roleName in userRoles)
            {
                // Fetch the role entity using the role name
                var role = await _roleManager.FindByNameAsync(roleName);
                if (role == null)
                {
                    throw new EntityNotFoundException(nameof(role));
                }

                // Add the role details to the DTO list
                roleDtos.Add(new GetRoleDTO { Id = role.Id, Name = role.Name! });
            }

            return roleDtos;
        }

        public async Task<bool> HasRolePermissionToEndpointAsync(string name, string code)
        {
            var userRoles = await GetRolesAssignedToUserAsync(name);

            var endpoint = await _appEndpointReadRepository.Table
                      .Include(e => e.Roles)
                      .FirstOrDefaultAsync(e => e.Code == code);

            if (endpoint == null)
                return false;

            var endpointRoles = endpoint.Roles.Select(r => r.Name);

            if (!endpointRoles.Any()) return true;

            if (userRoles.Count == 0)
                return false;


            foreach (var userRole in userRoles)
            {
                foreach (var endpointRole in endpointRoles)
                    if (userRole.Name == endpointRole)
                        return true;
            }

            return false;
        }
    }
}
