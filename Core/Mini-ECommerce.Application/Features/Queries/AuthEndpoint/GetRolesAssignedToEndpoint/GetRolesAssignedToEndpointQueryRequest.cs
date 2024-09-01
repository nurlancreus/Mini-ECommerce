using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mini_ECommerce.Application.Features.Queries.AuthEndpoint.GetRolesAssignedToEndpoint
{
    public class GetRolesAssignedToEndpointQueryRequest : IRequest<GetRolesAssignedToEndpointQueryResponse>
    {
        public string Code { get; set; }
        public string Menu { get; set; }
    }
}
