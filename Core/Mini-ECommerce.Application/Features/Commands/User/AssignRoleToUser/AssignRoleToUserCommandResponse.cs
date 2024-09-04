using MediatR;
using Mini_ECommerce.Application.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mini_ECommerce.Application.Features.Commands.User.AssignRoleToUser
{
    public class AssignRoleToUserCommandResponse : BaseResponse, IRequest<AssignRoleToUserCommandRequest>
    {
    }
}
