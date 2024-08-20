using Mini_ECommerce.Application.DTOs.Token;
using Mini_ECommerce.Domain.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mini_ECommerce.Application.Abstractions.Services.Token
{
    public interface IAppTokenHandler
    {
        TokenDTO CreateAccessToken(int second, AppUser appUser);
        string CreateRefreshToken();
    }
}
