using Mini_ECommerce.Application.DTOs.Role;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mini_ECommerce.Application.Abstractions.Services
{
    public interface IRoleService
    {
        Task<GetAllRolesDTO> GetAllRolesAsync(int page, int size);
        Task<GetRoleDTO> GetRoleByIdAsync(string id);
        Task CreateRoleAsync(string name);
        Task DeleteRoleAsync(string id);
        Task UpdateRoleAsync(string id, string name);
    }
}
