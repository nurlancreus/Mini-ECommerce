using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mini_ECommerce.Application.Features.Commands.Role.DeleteRole
{
    public class DeleteRoleCommandResponse : IRequest<DeleteRoleCommandRequest>
    {
        public bool Success { get; set; }
        public string? Message { get; set; }
    }
}
