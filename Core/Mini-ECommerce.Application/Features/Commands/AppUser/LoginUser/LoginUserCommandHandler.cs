using MediatR;
using Microsoft.AspNetCore.Identity;
using Mini_ECommerce.Application.Abstractions.Services.Token;
using Mini_ECommerce.Application.Exceptions;
using System.Threading;
using System.Threading.Tasks;

namespace Mini_ECommerce.Application.Features.Commands.AppUser.LoginUser
{
    public class LoginUserCommandHandler : IRequestHandler<LoginUserCommandRequest, LoginUserCommandResponse>
    {
        private readonly SignInManager<Domain.Entities.Identity.AppUser> _signInManager;
        private readonly UserManager<Domain.Entities.Identity.AppUser> _userManager;
        private readonly IAppTokenHandler _tokenHandler;

        public LoginUserCommandHandler(SignInManager<Domain.Entities.Identity.AppUser> signInManager, UserManager<Domain.Entities.Identity.AppUser> userManager, IAppTokenHandler tokenHandler)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _tokenHandler = tokenHandler;
        }

        public async Task<LoginUserCommandResponse> Handle(LoginUserCommandRequest request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByEmailAsync(request.Email)
                        ?? throw new LoginException("Invalid login attempt.");

            var signInResult = await _signInManager.PasswordSignInAsync(user.UserName!, request.Password, request.RememberMe, lockoutOnFailure: false);

            if (!signInResult.Succeeded)
            {
                throw new LoginException("Invalid login attempt.");
            }

            return new LoginUserSuccessCommandResponse
            {
                Token = _tokenHandler.CreateAccessToken(5000, user)
            };
        }
    }
}
