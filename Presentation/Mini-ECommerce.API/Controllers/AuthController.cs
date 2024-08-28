using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Mini_ECommerce.Application.Exceptions;
using Mini_ECommerce.Application.Features.Commands.AppUser.FacebookLoginUser;
using Mini_ECommerce.Application.Features.Commands.AppUser.GoogleLoginUser;
using Mini_ECommerce.Application.Features.Commands.AppUser.LoginUser;
using Mini_ECommerce.Application.Features.Commands.AppUser.RefreshTokenLogin;
using Mini_ECommerce.Application.Features.Commands.AppUser.RegisterUser;
using Mini_ECommerce.Application.Features.Commands.AppUser.ResetPassword;
using Mini_ECommerce.Application.Features.Commands.AppUser.VerifyResetToken;

namespace Mini_ECommerce.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AuthController(IMediator mediator)
        {
            _mediator = mediator;
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

        [HttpPost("refresh-login")]
        public async Task<IActionResult> RefreshTokenLogin(RefreshTokenLoginCommandRequest refreshTokenLoginCommandRequest)
        {
            try
            {
                var response = await _mediator.Send(refreshTokenLoginCommandRequest);
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

        [HttpPost("login-facebook")]
        public async Task<IActionResult> FacebookLogin(FacebookLoginUserCommandRequest facebookLoginUserCommandRequest)
        {
            try
            {
                var response = await _mediator.Send(facebookLoginUserCommandRequest);
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

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordCommandRequest resetPasswordCommandRequest)
        {
            var response = await _mediator.Send(resetPasswordCommandRequest);
            return Ok(response);
        }

        [HttpPost("verify-reset-token")]
        public async Task<IActionResult> VerifyResetToken([FromBody] VerifyResetTokenCommandRequest verifyResetTokenCommandRequest)
        {
            var response = await _mediator.Send(verifyResetTokenCommandRequest);
            return Ok(response);
        }
    }
}
