using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mini_ECommerce.Application.Features.Commands.AppUser.RegisterUser
{
    public class RegisterUserCommandResponse : IRequest<RegisterUserCommandRequest>
    {
        public string? Message {  get; set; } 
    }
}
