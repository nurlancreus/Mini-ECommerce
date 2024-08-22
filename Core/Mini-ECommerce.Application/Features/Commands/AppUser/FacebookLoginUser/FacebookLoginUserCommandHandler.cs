using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Mini_ECommerce.Application.Abstractions.Services.Auth;
using Mini_ECommerce.Application.Abstractions.Services.Token;
using Mini_ECommerce.Application.DTOs.FacebookAccess;
using Mini_ECommerce.Application.DTOs.Token;
using Mini_ECommerce.Application.Exceptions;
using Mini_ECommerce.Application.Features.Commands.AppUser.GoogleLoginUser;
using Mini_ECommerce.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Mini_ECommerce.Application.Features.Commands.AppUser.FacebookLoginUser
{
    public class FacebookLoginUserCommandHandler : IRequestHandler<FacebookLoginUserCommandRequest, FacebookLoginUserCommandResponse>
    {
        private readonly IExternalAuthService _externalAuthService;

        public FacebookLoginUserCommandHandler(IExternalAuthService externalAuthService)
        {
            _externalAuthService = externalAuthService;
        }

        public async Task<FacebookLoginUserCommandResponse> Handle(FacebookLoginUserCommandRequest request, CancellationToken cancellationToken)
        {

            var token = await _externalAuthService.FacebookLoginAsync(request.AuthToken, ExternalLoginProvider.Facebook.ToString(), 10000);

            return new FacebookLoginUserCommandResponse()
            {
                Token = new()
                {
                    AccessToken = token.AccessToken,
                }
            };
        }
    }
}
