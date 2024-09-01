using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Mini_ECommerce.Application.Features.Commands.AuthEndpoint.AssignRoleEndpoint;
using Mini_ECommerce.Application.Features.Queries.AuthEndpoint.GetRolesAssignedToEndpoint;

namespace Mini_ECommerce.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthEndpointsController : ControllerBase
    {
        readonly IMediator _mediator;

        public AuthEndpointsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetRolesToEndpoint([FromQuery]GetRolesAssignedToEndpointQueryRequest rolesToEndpointQueryRequest)
        {
            var response = await _mediator.Send(rolesToEndpointQueryRequest);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> AssignRoleEndpoint(AssignRoleEndpointCommandRequest assignRoleEndpointCommandRequest)
        {
            assignRoleEndpointCommandRequest.Type = typeof(Program);

            var response = await _mediator.Send(assignRoleEndpointCommandRequest);
            return Ok(response);
        }
    }
}
