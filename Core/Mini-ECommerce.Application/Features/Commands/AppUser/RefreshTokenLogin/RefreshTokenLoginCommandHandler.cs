﻿using MediatR;
using Mini_ECommerce.Application.Abstractions.Services.Auth;
using Mini_ECommerce.Application.DTOs.Token;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mini_ECommerce.Application.Features.Commands.AppUser.RefreshTokenLogin
{
    public class RefreshTokenLoginCommandHandler : IRequestHandler<RefreshTokenLoginCommandRequest, RefreshTokenLoginCommandResponse>
    {
        readonly IInternalAuthService _internalAuthService;

        public RefreshTokenLoginCommandHandler(IInternalAuthService internalAuthService)
        {
            _internalAuthService = internalAuthService;
        }

        public async Task<RefreshTokenLoginCommandResponse> Handle(RefreshTokenLoginCommandRequest request, CancellationToken cancellationToken)
        {
            TokenDTO token = await _internalAuthService.RefreshTokenLoginAsync(request.AccessToken, request.RefreshToken);

            return new()
            {
                Token = token
            };
        }
    }
}
