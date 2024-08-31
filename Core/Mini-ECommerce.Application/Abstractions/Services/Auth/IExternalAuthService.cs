using Microsoft.AspNetCore.Identity;
using Mini_ECommerce.Application.Abstractions.Services.Token;
using Mini_ECommerce.Application.DTOs.Token;
using Mini_ECommerce.Application.DTOs.User;
using Mini_ECommerce.Domain.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mini_ECommerce.Application.Abstractions.Services.Auth
{
    public interface IExternalAuthService
    {
        Task<TokenDTO> GoogleLoginAsync(string idToken, string provider);
        Task<TokenDTO> FacebookLoginAsync(string authToken, string provider);
    }
}
