using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mini_ECommerce.Application.Features.Queries.User.GetRolesAssignedToUser
{
    public class GetRolesAssignedToUserQueryHandler : IRequestHandler<GetRolesAssignedToUserQueryRequest, GetRolesAssignedToUserQueryResponse>
    {
        public Task<GetRolesAssignedToUserQueryResponse> Handle(GetRolesAssignedToUserQueryRequest request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
