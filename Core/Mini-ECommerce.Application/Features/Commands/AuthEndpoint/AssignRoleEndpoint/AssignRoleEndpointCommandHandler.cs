using MediatR;
using Mini_ECommerce.Application.Abstractions.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mini_ECommerce.Application.Features.Commands.AuthEndpoint.AssignRoleEndpoint
{
    public class AssignRoleEndpointCommandHandler : IRequestHandler<AssignRoleEndpointCommandRequest, AssignRoleEndpointCommandResponse>
    {
        private readonly IAuthEndpointService _authEndpointService;

        public AssignRoleEndpointCommandHandler(IAuthEndpointService authEndpointService)
        {
            _authEndpointService = authEndpointService;
        }

        public async Task<AssignRoleEndpointCommandResponse> Handle(AssignRoleEndpointCommandRequest request, CancellationToken cancellationToken)
        {
            var endpointCode = await _authEndpointService.AssignRoleEndpointAsync(request.Roles, request.Menu, request.Code, request.Type!);

            return new AssignRoleEndpointCommandResponse()
            {
                Success = true,
                Message = $"Roles have been added to the endpoint '{endpointCode}'"
            };
        }
    }
}
