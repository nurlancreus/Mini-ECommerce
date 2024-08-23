using MediatR;
using Microsoft.AspNetCore.Identity;
using Mini_ECommerce.Application.Abstractions.Services;
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
        private readonly IUserService _userService;
        public RegisterUserCommandHandler(IUserService userService)
        {
            _userService = userService;
        }

        public async Task<RegisterUserCommandResponse> Handle(RegisterUserCommandRequest request, CancellationToken cancellationToken)
        {

            var response = await _userService.RegisterUserAsync(new()
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                Email = request.Email,
                UserName = request.UserName,
                Password = request.Password,
                ConfirmPassword = request.ConfirmPassword,
            });

            return new RegisterUserCommandResponse()
            {
                Message = response.Message,
                Success = response.Success,
                Token = response.Token
            };
        }
    }
}
