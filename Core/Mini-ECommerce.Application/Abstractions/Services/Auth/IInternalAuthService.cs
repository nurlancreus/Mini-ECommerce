using Mini_ECommerce.Application.DTOs.Token;
using Mini_ECommerce.Application.DTOs.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mini_ECommerce.Application.Abstractions.Services.Auth
{
    public interface IInternalAuthService : IAuthService
    {
        Task<LoginUserResponseDTO> LoginAsync(LoginUserRequestDTO loginUserRequestDTO);
        Task<TokenDTO> RefreshTokenLoginAsync(string accessToken, string refreshToken);
    }
}
