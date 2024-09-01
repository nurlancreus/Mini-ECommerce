using MediatR;
using Microsoft.AspNetCore.Identity;
using Mini_ECommerce.Application.Abstractions.Services.Auth;
using Mini_ECommerce.Application.Abstractions.Services.Token;
using Mini_ECommerce.Application.Exceptions;
using System.Threading;
using System.Threading.Tasks;

namespace Mini_ECommerce.Application.Features.Commands.AppUser.LoginUser
{
    public class LoginUserCommandHandler : IRequestHandler<LoginUserCommandRequest, LoginUserCommandResponse>
    {

        private readonly IInternalAuthService _authService;

        public LoginUserCommandHandler(IInternalAuthService authService)
        {
            _authService = authService;
        }

        public async Task<LoginUserCommandResponse> Handle(LoginUserCommandRequest request, CancellationToken cancellationToken)
        {
            var response = await _authService.LoginAsync(new()
            {
                Email = request.Email,
                Password = request.Password,
                RememberMe = request.RememberMe,
            });

            return new LoginUserSuccessCommandResponse
            {
                Token = response.Token,
                Message = response.Message,
            };
        }
    }
}
