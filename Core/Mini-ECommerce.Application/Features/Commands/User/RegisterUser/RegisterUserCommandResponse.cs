using MediatR;
using Mini_ECommerce.Application.DTOs.Token;
using Mini_ECommerce.Application.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mini_ECommerce.Application.Features.Commands.User.RegisterUser
{
    public class RegisterUserCommandResponse :BaseResponse, IRequest<RegisterUserCommandRequest>
    {
        public TokenDTO Token { get; set; }
    }
}
