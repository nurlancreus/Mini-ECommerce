using FluentValidation;
using Mini_ECommerce.Application.Features.Commands.AppUser.UpdatePassword;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mini_ECommerce.Application.Validators.UpdatePassword
{
    public class UpdatePasswordCommandValidator : AbstractValidator<UpdatePasswordCommandRequest>
    {
        public UpdatePasswordCommandValidator()
        {
            RuleFor(u => u.ConfirmPassword)
                .Equal(u => u.Password).WithMessage("Passwords do not match.");
        }
    }
}
