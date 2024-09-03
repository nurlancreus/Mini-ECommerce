using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Mini_ECommerce.Application.Abstractions.Services;
using Mini_ECommerce.Application.Features.Commands.AuthEndpoint.AssignRoleEndpoint;
using Mini_ECommerce.Application.Helpers;
using Mini_ECommerce.Domain.Entities.Identity;
using Mini_ECommerce.Domain.Enums;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Mini_ECommerce.Application.Validators.AuthEndpoint
{
    public class AssignRoleToEndpointCommandRequestValidator : AbstractValidator<AssignRoleToEndpointCommandRequest>
    {
        private readonly RoleManager<AppRole> _roleManager;

        public AssignRoleToEndpointCommandRequestValidator(RoleManager<AppRole> roleManager)
        {
            _roleManager = roleManager;

            RuleFor(e => e.Menu)
                .NotEmpty()
                .NotNull()
                .WithMessage("Menu name is required.")
                .Must(IsMenuExist)
                .WithMessage("There is no such menu");

            RuleFor(e => e.Roles)
                .MustAsync(ContainValidRolesAsync)
                .WithMessage("Roles list contains invalid roles.");

            RuleFor(e => e)
                .MustAsync(MenuMatchesCodeAsync)
                .WithMessage("The menu provided does not match the menu in the code.");
        }

        private async Task<bool> ContainValidRolesAsync(string[] roles, CancellationToken cancellationToken)
        {
            foreach (var role in roles)
            {
                if (!EnumHelpers.TryParseEnum(role, out Role _) || !await _roleManager.RoleExistsAsync(role))
                {
                    return false;
                }
            }
            return true;
        }

        private bool IsMenuExist(string menuName)
        {
            return EnumHelpers.TryParseEnum(menuName, out AuthorizedMenu _);
        }

        private async Task<bool> MenuMatchesCodeAsync(AssignRoleToEndpointCommandRequest request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(request.Code))
            {
                return false;
            }

            // Split the code and ensure it has the correct format
            string[] codeParts = request.Code.Split('.');
            if (codeParts.Length < 4)
            {
                return false;
            }

            // Extract menuName from the code
            string menuName = codeParts[1];

            // Compare the extracted menuName with the provided menu, case-insensitive
            return await Task.FromResult(menuName.Equals(request.Menu, StringComparison.CurrentCultureIgnoreCase));
        }
    }
}
