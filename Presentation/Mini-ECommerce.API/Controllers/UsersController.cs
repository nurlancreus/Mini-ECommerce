using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Mini_ECommerce.Application.Attributes;
using Mini_ECommerce.Application.Exceptions;
using Mini_ECommerce.Application.Features.Commands.User.FacebookLoginUser;
using Mini_ECommerce.Application.Features.Commands.User.GoogleLoginUser;
using Mini_ECommerce.Application.Features.Commands.User.LoginUser;
using Mini_ECommerce.Application.Features.Commands.User.RegisterUser;
using Mini_ECommerce.Application.Features.Commands.User.UpdatePassword;
using Mini_ECommerce.Application.Features.Commands.User.AssignRoleToUser;
using Mini_ECommerce.Application.Features.Queries.AuthEndpoint.GetRolesAssignedToEndpoint;
using Mini_ECommerce.Application.Features.Queries.User.GetAllUsers;
using Mini_ECommerce.Application.Features.Queries.User.GetRolesAssignedToUser;
using Mini_ECommerce.Domain.Enums;

namespace Mini_ECommerce.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IMediator _mediator;

        public UsersController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterUserCommandRequest registerUserCommandRequest)
        {
            var response = await _mediator.Send(registerUserCommandRequest);

            return Ok(response);

        }

        [HttpPost("update-password")]
        public async Task<IActionResult> UpdatePassword([FromBody] UpdatePasswordCommandRequest updatePasswordCommandRequest)
        {
            var response = await _mediator.Send(updatePasswordCommandRequest);
            return Ok(response);
        }

        [HttpGet]
        // [Authorize(AuthenticationSchemes = "Admin")]
        [AuthorizeDefinition(ActionType = ActionType.Reading, Definition = "Get All Users", Menu = AuthorizedMenu.Users)]
        public async Task<IActionResult> GetAllUsers([FromQuery] GetAllUsersQueryRequest getAllUsersQueryRequest)
        {
            var response = await _mediator.Send(getAllUsersQueryRequest);
            return Ok(response);
        }

        [HttpGet("get-roles-to-user/{UserId}")]
        // [Authorize(AuthenticationSchemes = "Admin")]
        [AuthorizeDefinition(ActionType = ActionType.Reading, Definition = "Get Roles To Users", Menu = AuthorizedMenu.Users)]
        public async Task<IActionResult> GetRolesToUser([FromRoute] GetRolesAssignedToUserQueryRequest getRolesToUserQueryRequest)
        {
            var response = await _mediator.Send(getRolesToUserQueryRequest);
            return Ok(response);
        }

        [HttpPost("assign-role-to-user")]
        // [Authorize(AuthenticationSchemes = "Admin")]
        [AuthorizeDefinition(ActionType = ActionType.Reading, Definition = "Assign Role To User", Menu = AuthorizedMenu.Users)]
        public async Task<IActionResult> AssignRoleToUser(AssignRoleToUserCommandRequest assignRoleToUserCommandRequest)
        {
            var response = await _mediator.Send(assignRoleToUserCommandRequest);
            return Ok(response);
        }
    }
}
