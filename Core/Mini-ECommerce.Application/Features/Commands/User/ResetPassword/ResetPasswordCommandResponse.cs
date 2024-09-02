using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mini_ECommerce.Application.Features.Commands.User.ResetPassword
{
    public class ResetPasswordCommandResponse : IRequest<ResetPasswordCommandRequest>
    {
        public bool Success { get; set; }
        public string? Message { get; set; }
    }
}
