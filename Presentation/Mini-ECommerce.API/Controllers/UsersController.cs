using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Mini_ECommerce.Application.Exceptions;
using Mini_ECommerce.Application.Features.Commands.AppUser.GoogleLoginUser;
using Mini_ECommerce.Application.Features.Commands.AppUser.LoginUser;
using Mini_ECommerce.Application.Features.Commands.AppUser.RegisterUser;

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
            try
            {
                var response = await _mediator.Send(registerUserCommandRequest);
                return Ok(response);
            }
            catch (RegistrationException ex)
            {
                return StatusCode((int)ex.StatusCode, ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Error = "An unexpected error occurred.", Details = ex.Message });
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginUserCommandRequest loginUserCommandRequest)
        {
            try
            {
                var response = await _mediator.Send(loginUserCommandRequest);
                return Ok(response);
            }
            catch (LoginException ex)
            {
                return StatusCode((int)ex.StatusCode, ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Error = "An unexpected error occurred.", Details = ex.Message });
            }
        }

        [HttpPost("login-google")]
        public async Task<IActionResult> GoogleLogin(GoogleLoginUserCommandRequest googleLoginUserCommandRequest)
        {
            try
            {
                var response = await _mediator.Send(googleLoginUserCommandRequest);
                return Ok(response);
            }
            catch (LoginException ex)
            {
                return StatusCode((int)ex.StatusCode, ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Error = "An unexpected error occurred.", Details = ex.Message });
            }
        }
    }
}
