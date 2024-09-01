using Mini_ECommerce.Application.DTOs.Role;
using Mini_ECommerce.Application.ViewModels.Role;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mini_ECommerce.Application.ViewModels.AuthEndpoint
{
    public class GetAuthEndpointVM
    {
        public List<GetRoleVM> Roles { get; set; } = [];
        public string Code { get; set; }
    }
}
