using MediatR;
using Mini_ECommerce.Application.DTOs.Token;
using Mini_ECommerce.Application.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mini_ECommerce.Application.Features.Commands.User.LoginUser
{
    public class LoginUserCommandResponse : BaseResponse, IRequest<LoginUserCommandRequest>
    {

    }

    public class LoginUserSuccessCommandResponse : LoginUserCommandResponse
    {
        public TokenDTO Token { get; set; }
    }
    public class LoginUserErrorCommandResponse : LoginUserCommandResponse
    {
    }
}
