using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mini_ECommerce.Application.DTOs.Role
{
    public class GetAllRolesDTO
    {
        public List<GetRoleDTO> Roles { get; set; }
        public int Count { get; set; }
    }
}
