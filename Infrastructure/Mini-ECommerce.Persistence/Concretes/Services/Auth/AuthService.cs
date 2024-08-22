using Mini_ECommerce.Application.Abstractions.Services.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mini_ECommerce.Persistence.Concretes.Services.Auth
{
    public class AuthService : IAuthService
    {
        public Task PasswordResetAsnyc(string email)
        {
            throw new NotImplementedException();
        }

        public Task<bool> VerifyResetTokenAsync(string resetToken, string userId)
        {
            throw new NotImplementedException();
        }
    }
}
