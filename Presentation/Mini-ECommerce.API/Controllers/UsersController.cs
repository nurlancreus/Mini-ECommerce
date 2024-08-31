//using MediatR;
//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Mvc;
//using Mini_ECommerce.Application.Exceptions;
//using Mini_ECommerce.Application.Features.Commands.AppUser.FacebookLoginUser;
//using Mini_ECommerce.Application.Features.Commands.AppUser.GoogleLoginUser;
//using Mini_ECommerce.Application.Features.Commands.AppUser.LoginUser;
//using Mini_ECommerce.Application.Features.Commands.AppUser.RegisterUser;
//using Mini_ECommerce.Application.Features.Commands.AppUser.UpdatePassword;

//namespace Mini_ECommerce.API.Controllers
//{
//    [Route("api/[controller]")]
//    [ApiController]
//    public class UsersController : ControllerBase
//    {
//        private readonly IMediator _mediator;

//        public UsersController(IMediator mediator)
//        {
//            _mediator = mediator;
//        }

//        [HttpPost("register")]
//        public async Task<IActionResult> Register(RegisterUserCommandRequest registerUserCommandRequest)
//        {
//            var response = await _mediator.Send(registerUserCommandRequest);

//            return Ok(response);

//        }

//        [HttpPost("update-password")]
//        public async Task<IActionResult> UpdatePassword([FromBody] UpdatePasswordCommandRequest updatePasswordCommandRequest)
//        {
//            var response = await _mediator.Send(updatePasswordCommandRequest);
//            return Ok(response);
//        }
//    }
//}
