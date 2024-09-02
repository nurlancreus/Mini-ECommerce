using MediatR;
using Mini_ECommerce.Application.DTOs.Token;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mini_ECommerce.Application.Features.Commands.User.RegisterUser
{
    public class RegisterUserCommandResponse : IRequest<RegisterUserCommandRequest>
    {
        public bool Success { get; set; }
        public string? Message {  get; set; } 
        public TokenDTO Token { get; set; }
    }
}
