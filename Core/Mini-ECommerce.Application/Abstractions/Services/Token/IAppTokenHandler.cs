﻿using Mini_ECommerce.Application.DTOs.Token;
using Mini_ECommerce.Domain.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Mini_ECommerce.Application.Abstractions.Services.Token
{
    public interface IAppTokenHandler
    {
        TokenDTO CreateAccessToken(AppUser appUser);
        string CreateRefreshToken();
        ClaimsPrincipal? GetPrincipalFromAccessToken(string? accessToken);
    }
}
