﻿using MediatR;
using Mini_ECommerce.Application.Abstractions.Services.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mini_ECommerce.Application.Features.Commands.User.ResetPassword
{
    public class ResetPasswordCommandHandler : IRequestHandler<ResetPasswordCommandRequest, ResetPasswordCommandResponse>
    {
        private readonly IAuthManagementService _authService;

        public ResetPasswordCommandHandler(IAuthManagementService authService)
        {
            _authService = authService;
        }

        public async Task<ResetPasswordCommandResponse> Handle(ResetPasswordCommandRequest request, CancellationToken cancellationToken)
        {
            await _authService.ResetPasswordAsync(request.Email);

            return new ResetPasswordCommandResponse()
            {
                Success = true,
            };
        }
    }
}
