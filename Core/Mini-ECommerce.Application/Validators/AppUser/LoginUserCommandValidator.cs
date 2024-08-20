using FluentValidation;
using Mini_ECommerce.Application.Features.Commands.AppUser.LoginUser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mini_ECommerce.Application.Validators.AppUser
{
    public class LoginUserCommandValidator : AbstractValidator<LoginUserCommandRequest>
    {
        public LoginUserCommandValidator()
        {
            RuleFor(u => u.Email)
                .NotEmpty().WithMessage("Username or Email is required.")
                .NotNull().WithMessage("Username or Email cannot be null.")
                .EmailAddress().WithMessage("Please enter a valid email address or username.");
            
            RuleFor(u => u.Password)
                .NotEmpty().WithMessage("Password is required.")
                .NotNull().WithMessage("Password cannot be null.")
                .MinimumLength(6).WithMessage("Password must be at least 6 characters long.");
        }
    }
}
