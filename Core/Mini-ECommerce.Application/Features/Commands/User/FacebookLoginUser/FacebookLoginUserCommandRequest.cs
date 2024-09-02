using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mini_ECommerce.Application.Features.Commands.User.FacebookLoginUser
{
    public class FacebookLoginUserCommandRequest : IRequest<FacebookLoginUserCommandResponse>
    {
        public string AuthToken { get; set; }
    }
}
