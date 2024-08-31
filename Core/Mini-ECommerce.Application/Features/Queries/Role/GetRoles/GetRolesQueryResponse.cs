using MediatR;
using Mini_ECommerce.Application.ViewModels.Role;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mini_ECommerce.Application.Features.Queries.Role.GetRoles
{
    public class GetRolesQueryResponse : IRequest<GetRolesQueryRequest>
    {
        public List<GetRoleVM> Roles { get; set; }
        public int TotalCount { get; set; }
        public string? Message { get; set; }
    }
}
