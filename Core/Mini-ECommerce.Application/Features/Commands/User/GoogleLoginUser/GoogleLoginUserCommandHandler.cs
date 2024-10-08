﻿using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Mini_ECommerce.Application.Abstractions.Services.Auth;
using Mini_ECommerce.Application.Abstractions.Services.Token;
using Mini_ECommerce.Application.DTOs.Token;
using Mini_ECommerce.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Mini_ECommerce.Application.Features.Commands.User.GoogleLoginUser
{
    public class GoogleLoginUserCommandHandler : IRequestHandler<GoogleLoginUserCommandRequest, GoogleLoginUserCommandResponse>
    {
        private readonly IExternalAuthService _authService;

        public GoogleLoginUserCommandHandler(IExternalAuthService authService)
        {
            _authService = authService;
        }

        public async Task<GoogleLoginUserCommandResponse> Handle(GoogleLoginUserCommandRequest request, CancellationToken cancellationToken)
        {

            var token = await _authService.GoogleLoginAsync(request.IdToken, nameof(ExternalLoginProvider));

            return new GoogleLoginUserCommandResponse()
            {
                Token = new()
                {
                    AccessToken = token.AccessToken,
                }
            };

        }
    }
}
