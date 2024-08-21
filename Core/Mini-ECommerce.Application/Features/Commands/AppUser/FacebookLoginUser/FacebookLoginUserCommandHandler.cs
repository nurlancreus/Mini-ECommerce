using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mini_ECommerce.Application.Features.Commands.AppUser.FacebookLoginUser
{
    public class FacebookLoginUserCommandHandler : IRequestHandler<FacebookLoginUserCommandRequest, FacebookLoginUserCommandResponse>
    {
        public Task<FacebookLoginUserCommandResponse> Handle(FacebookLoginUserCommandRequest request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
