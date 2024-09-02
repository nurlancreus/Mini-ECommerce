using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Mini_ECommerce.Application.Abstractions.Repositories;
using Mini_ECommerce.Application.Features.Commands.User.RegisterUser;
using Mini_ECommerce.Domain.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mini_ECommerce.Application.Validators.AppUser
{
    public class RegisterUserCommandValidator : AbstractValidator<RegisterUserCommandRequest>
    {
        private readonly UserManager<Domain.Entities.Identity.AppUser> _userManager;
        public RegisterUserCommandValidator(UserManager<Domain.Entities.Identity.AppUser> userManager)
        {
            _userManager = userManager;

            RuleFor(u => u.FirstName)
                 .NotEmpty().WithMessage("First Name is required.")
                 .NotNull().WithMessage("First Name cannot be null.")
                 .MinimumLength(2).WithMessage("First Name must be at least 2 characters long.")
                 .MaximumLength(50).WithMessage("First Name cannot exceed 50 characters.");

            RuleFor(u => u.LastName)
                .NotEmpty().WithMessage("Last Name is required.")
                .NotNull().WithMessage("Last Name cannot be null.")
                .MinimumLength(2).WithMessage("Last Name must be at least 2 characters long.")
                .MaximumLength(50).WithMessage("Last Name cannot exceed 50 characters.");

            RuleFor(u => u.UserName)
                .NotEmpty().WithMessage("Username is required.")
                .NotNull().WithMessage("Username cannot be null.")
                .MinimumLength(5).WithMessage("Username must be at least 5 characters long.")
                .MaximumLength(20).WithMessage("Username cannot exceed 20 characters.")
                .MustAsync(async (username, cancellation) =>
                {
                    var user = await _userManager.FindByNameAsync(username);

                    return user == null;
                }).WithMessage(data => $"The username '{data.UserName}' is already in use. Please use a different email.");
            // Add unique username validation logic here, if needed.

            RuleFor(u => u.Email)
                .NotEmpty().WithMessage("Email is required.")
                .NotNull().WithMessage("Email cannot be null.")
                .EmailAddress().WithMessage("A valid email is required.")
                .MustAsync(async (email, cancellation) =>
                {
                    var user = await _userManager.FindByEmailAsync(email);

                    return user == null;
                }).WithMessage(data => $"The email '{data.Email}' is already in use. Please use a different email.");

            RuleFor(u => u.Password)
                .NotEmpty().WithMessage("Password is required.")
                .NotNull().WithMessage("Password cannot be null.");
                //.MinimumLength(8).WithMessage("Password must be at least 8 characters long.")
                //.Matches(@"[A-Z]").WithMessage("Password must contain at least one uppercase letter.")
                //.Matches(@"[a-z]").WithMessage("Password must contain at least one lowercase letter.")
                //.Matches(@"\d").WithMessage("Password must contain at least one digit.")
                //.Matches(@"[\!\?\*\.]").WithMessage("Password must contain at least one special character (!? *.).");

            RuleFor(u => u.ConfirmPassword)
                .Equal(u => u.Password).WithMessage("Passwords do not match.");

        }
    }
}
