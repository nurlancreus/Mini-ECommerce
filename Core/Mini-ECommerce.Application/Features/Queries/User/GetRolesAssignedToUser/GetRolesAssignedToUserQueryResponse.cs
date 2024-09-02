using MediatR;
using Mini_ECommerce.Application.ViewModels.Role;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mini_ECommerce.Application.Features.Queries.User.GetRolesAssignedToUser
{
    public class GetRolesAssignedToUserQueryResponse : IRequest<GetRolesAssignedToUserQueryRequest>
    {
        public bool Success { get; set; }
        public List<GetRoleVM> Roles { get; set; } = [];
        public string? Message { get; set; }
    }
}
