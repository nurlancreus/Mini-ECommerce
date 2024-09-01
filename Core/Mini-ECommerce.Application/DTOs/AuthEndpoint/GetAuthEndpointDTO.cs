using Mini_ECommerce.Application.DTOs.Role;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mini_ECommerce.Application.DTOs.AuthEndpoint
{
    public class GetAuthEndpointDTO
    {
        public List<GetRoleDTO> Roles { get; set; } = [];
        public string Code { get; set; }
    }
}
