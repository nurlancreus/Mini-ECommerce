using MediatR;
using Mini_ECommerce.Application.Abstractions.Services;
using Mini_ECommerce.Application.ViewModels.AuthEndpoint;
using Mini_ECommerce.Application.ViewModels.Role;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mini_ECommerce.Application.Features.Queries.AuthEndpoint.GetRolesAssignedToEndpoint
{
    public class GetRolesAssignedToEndpointQueryHandler : IRequestHandler<GetRolesAssignedToEndpointQueryRequest, GetRolesAssignedToEndpointQueryResponse>
    {
        private readonly IAuthEndpointService _authEndpointService;

        public GetRolesAssignedToEndpointQueryHandler(IAuthEndpointService authEndpointService)
        {
            _authEndpointService = authEndpointService;
        }

        public async Task<GetRolesAssignedToEndpointQueryResponse> Handle(GetRolesAssignedToEndpointQueryRequest request, CancellationToken cancellationToken)
        {
            var result = await _authEndpointService.GetRolesToEndpointAsync(request.Code, request.Menu);

            return new GetRolesAssignedToEndpointQueryResponse()
            {
                Success = true,
                Endpoint = new GetAuthEndpointVM()
                {
                    Code = result.Code,
                    Roles = result.Roles.Select(r => new GetRoleVM()
                    {
                        Id = r.Id,
                        Name = r.Name,
                    }).ToList(),
                }
            };
        }
    }
}
