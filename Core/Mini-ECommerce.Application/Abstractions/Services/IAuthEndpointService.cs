using Mini_ECommerce.Application.DTOs.AuthEndpoint;
using Mini_ECommerce.Application.DTOs.Role;

namespace Mini_ECommerce.Application.Abstractions.Services
{
    public interface IAuthEndpointService
    {
        public Task<string> AssignRoleEndpointAsync(string[] roles, string menu, string code, Type type);
        public Task<GetAuthEndpointDTO> GetRolesToEndpointAsync(string code, string menu);
    }
}
