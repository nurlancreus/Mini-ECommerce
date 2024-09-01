using MediatR;
using Mini_ECommerce.Application.Abstractions.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mini_ECommerce.Application.Features.Commands.AuthEndpoint.AssignRoleEndpoint
{
    public class AssignRoleToEndpointCommandHandler : IRequestHandler<AssignRoleToEndpointCommandRequest, AssignRoleToEndpointCommandResponse>
    {
        private readonly IAuthEndpointService _authEndpointService;

        public AssignRoleToEndpointCommandHandler(IAuthEndpointService authEndpointService)
        {
            _authEndpointService = authEndpointService;
        }

        public async Task<AssignRoleToEndpointCommandResponse> Handle(AssignRoleToEndpointCommandRequest request, CancellationToken cancellationToken)
        {
            var endpointCode = await _authEndpointService.AssignRoleEndpointAsync(request.Roles, request.Menu, request.Code, request.Type!);

            return new AssignRoleToEndpointCommandResponse()
            {
                Success = true,
                Message = $"Roles have been modified on the endpoint '{endpointCode}'"
            };
        }
    }
}
