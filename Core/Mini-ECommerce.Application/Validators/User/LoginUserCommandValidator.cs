using FluentValidation;
using Mini_ECommerce.Application.Features.Commands.User.LoginUser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mini_ECommerce.Application.Validators.User
{
    public class LoginUserCommandValidator : AbstractValidator<LoginUserCommandRequest>
    {
        public LoginUserCommandValidator()
        {
            RuleFor(u => u.Email)
                .NotEmpty().WithMessage("Email is required.")
                .NotNull().WithMessage("Email cannot be null.")
                .EmailAddress().WithMessage("Please enter a valid email address.");
            
            RuleFor(u => u.Password)
                .NotEmpty().WithMessage("Password is required.")
                .NotNull().WithMessage("Password cannot be null.")
                .MinimumLength(6).WithMessage("Password must be at least 6 characters long.");
        }
    }
}
