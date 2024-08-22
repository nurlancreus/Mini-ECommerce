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

        private readonly IInternalAuthService _internalAuthService;

        public LoginUserCommandHandler(IInternalAuthService internalAuthService)
        {
            _internalAuthService = internalAuthService;
        }

        public async Task<LoginUserCommandResponse> Handle(LoginUserCommandRequest request, CancellationToken cancellationToken)
        {
            var response = await _internalAuthService.LoginAsync(new()
            {
                Email = request.Email,
                Password = request.Password,
                RememberMe = request.RememberMe,
            }, 10000);

            return new LoginUserSuccessCommandResponse
            {
                Token = response.Token,
                Message = response.Message,
            };
        }
    }
}
