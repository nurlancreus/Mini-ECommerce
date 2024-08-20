using MediatR;
using Microsoft.AspNetCore.Identity;
using Mini_ECommerce.Application.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mini_ECommerce.Application.Features.Commands.AppUser.RegisterUser
{
    public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommandRequest, RegisterUserCommandResponse>
    {
        private readonly UserManager<Domain.Entities.Identity.AppUser> _userManager;

        public RegisterUserCommandHandler(UserManager<Domain.Entities.Identity.AppUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<RegisterUserCommandResponse> Handle(RegisterUserCommandRequest request, CancellationToken cancellationToken)
        {

            var result = await _userManager.CreateAsync(new () { FirstName = request.FirstName, LastName = request.LastName, UserName = request.UserName, Email = request.Email }, request.Password);

            if(!result.Succeeded)
            {
                string message = string.Empty;

                foreach (var error in result.Errors)
                {
                    message += $"{error.Code} - {error.Description}\n";
                }

                throw new RegistrationException(message);
            }

            return new RegisterUserCommandResponse() { Message = "User created successfully"};
        }
    }
}
