using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mini_ECommerce.Application.Abstractions.Services.Auth
{
    public interface IAuthService
    {
        Task ResetPasswordAsnyc(string email);
        Task<bool> VerifyResetTokenAsync(string resetToken, string userId);
    }
}
