using Microsoft.AspNetCore.Identity;
using Mini_ECommerce.Application.Abstractions.Services;
using Mini_ECommerce.Application.Abstractions.Services.Auth;
using Mini_ECommerce.Application.Extensions;
using Mini_ECommerce.Domain.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mini_ECommerce.Persistence.Concretes.Services.Auth
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IMailService _mailService;

        public AuthService(UserManager<AppUser> userManager, IMailService mailService)
        {
            _userManager = userManager;
            _mailService = mailService;
        }

        public async Task ResetPasswordAsnyc(string email)
        {
            AppUser? user = await _userManager.FindByEmailAsync(email);
            if (user != null)
            {
                string resetToken = await _userManager.GeneratePasswordResetTokenAsync(user);

                //byte[] tokenBytes = Encoding.UTF8.GetBytes(resetToken);
                //resetToken = WebEncoders.Base64UrlEncode(tokenBytes);
                resetToken = resetToken.UrlEncode();

                await _mailService.SendPasswordResetMailAsync(user.Email!, user.Id, resetToken);
            }
        }

        public async Task<bool> VerifyResetTokenAsync(string resetToken, string userId)
        {
            AppUser? user = await _userManager.FindByIdAsync(userId);
            if (user != null)
            {
                //byte[] tokenBytes = WebEncoders.Base64UrlDecode(resetToken);
                //resetToken = Encoding.UTF8.GetString(tokenBytes);
                resetToken = resetToken.UrlDecode();

                return await _userManager.VerifyUserTokenAsync(user, _userManager.Options.Tokens.PasswordResetTokenProvider, "ResetPassword", resetToken);
            }
            return false;
        }
    }
}
