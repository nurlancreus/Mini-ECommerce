using MediatR;
using Mini_ECommerce.Application.DTOs.Token;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mini_ECommerce.Application.Features.Commands.AppUser.LoginUser
{
    public class LoginUserCommandResponse : IRequest<LoginUserCommandRequest>
    {
     
    }

    public class LoginUserSuccessCommandResponse : LoginUserCommandResponse
    {
        public TokenDTO Token { get; set; }
    }
    public class LoginUserErrorCommandResponse : LoginUserCommandResponse
    {
        public string Message { get; set; }
    }
}
