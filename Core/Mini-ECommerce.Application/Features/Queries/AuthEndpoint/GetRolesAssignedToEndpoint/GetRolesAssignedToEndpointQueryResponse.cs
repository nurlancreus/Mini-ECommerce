using MediatR;
using Mini_ECommerce.Application.ViewModels.AuthEndpoint;
using Mini_ECommerce.Application.ViewModels.Role;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mini_ECommerce.Application.Features.Queries.AuthEndpoint.GetRolesAssignedToEndpoint
{
    public class GetRolesAssignedToEndpointQueryResponse : IRequest<GetRolesAssignedToEndpointQueryRequest>
    {
        public bool Success { get; set; }
        public GetAuthEndpointVM Endpoint { get; set; }
        public string? Message { get; set; }
    }
}
