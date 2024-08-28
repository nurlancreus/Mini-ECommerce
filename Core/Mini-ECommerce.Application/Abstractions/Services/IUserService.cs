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
        Task<List<GetAppUserDTO>> GetAllUsersAsync(int page, int size);
        int TotalUsersCount { get; }
        Task AssignRoleToUserAsnyc(string userId, string[] roles);
        Task<string[]> GetRolesToUserAsync(string userIdOrName);
        Task<bool> HasRolePermissionToEndpointAsync(string name, string code);
    }
}
