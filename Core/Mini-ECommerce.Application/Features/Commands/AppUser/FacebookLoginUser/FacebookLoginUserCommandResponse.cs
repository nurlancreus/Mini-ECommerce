using MediatR;
using Mini_ECommerce.Application.DTOs.Token;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mini_ECommerce.Application.Features.Commands.AppUser.FacebookLoginUser
{
    public class FacebookLoginUserCommandResponse : IRequest<FacebookLoginUserCommandRequest>
    {
        public TokenDTO Token { get; set; }

    }
}
