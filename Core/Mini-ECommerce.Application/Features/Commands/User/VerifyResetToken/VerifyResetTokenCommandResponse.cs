using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mini_ECommerce.Application.Features.Commands.User.VerifyResetToken
{
    public class VerifyResetTokenCommandResponse : IRequest<VerifyResetTokenCommandRequest>
    {
        public bool State { get; set; }
    }
}
