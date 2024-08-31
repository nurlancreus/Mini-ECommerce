using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Mini_ECommerce.Application.Enums;
using Mini_ECommerce.Application.Features.Commands.Role.UpdateRole;
using Mini_ECommerce.Domain.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Mini_ECommerce.Application.Validators
{
    public class UpdateRoleCommandRequestValidator : AbstractValidator<UpdateRoleCommandRequest>
    {
        private readonly RoleManager<AppRole> _roleManager;

        public UpdateRoleCommandRequestValidator(RoleManager<AppRole> roleManager)
        {
            _roleManager = roleManager;

            RuleFor(r => r.Name)
                .NotEmpty().WithMessage("Role name is required.")
                .Must(name => IsValidRoleName(name.Trim()))
                .WithMessage("Roles must be defined in the application before being updated.")
                .MustAsync((name, cancellation) => IsUniqueRoleName(name.Trim(), cancellation))
                .WithMessage("A role with this name already exists.");
        }

        private static bool IsValidRoleName(string name)
        {
            return Enum.TryParse<Role>(name, true, out var roleEnumValue) && Enum.IsDefined(typeof(Role), roleEnumValue);
        }

        private async Task<bool> IsUniqueRoleName(string name, CancellationToken cancellationToken)
        {
            var role = await _roleManager.FindByNameAsync(name);
            return role == null;
        }
    }
}
