using Mini_ECommerce.Application.DTOs.Role;
using Mini_ECommerce.Application.DTOs.User;
using Mini_ECommerce.Domain.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mini_ECommerce.Application.Abstractions.Services
{
    public interface IUserService
    {
        Task<RegisterUserResponseDTO> RegisterUserAsync(RegisterUserRequestDTO userRequestDTO);
        Task UpdateRefreshTokenAsync(string refreshToken, AppUser user, DateTime accessTokenLifeTime);
        Task UpdatePasswordAsync(string userId, string resetToken, string newPassword);
        Task<GetAllUsersDTO> GetAllUsersAsync(int page, int size);
        Task AssignRoleToUserAsync(string userId, string[] roles);
        Task<List<GetRoleDTO>> GetRolesAssignedToUserAsync(string userIdOrName);
        Task<bool> HasRolePermissionToEndpointAsync(string name, string code);
    }
}
