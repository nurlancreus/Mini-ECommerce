using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Mini_ECommerce.Application.Abstractions.Repositories;
using Mini_ECommerce.Application.Enums;
using Mini_ECommerce.Application.Exceptions;
using Mini_ECommerce.Application.Features.Commands.Role.CreateRole;
using Mini_ECommerce.Domain.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mini_ECommerce.Application.Validators
{
    public class CreateRoleCommandRequestValidator : AbstractValidator<CreateRoleCommandRequest>
    {
        private readonly RoleManager<AppRole> _roleManager;

        public CreateRoleCommandRequestValidator(RoleManager<AppRole> roleManager)
        {
            _roleManager = roleManager;

            RuleFor(r => r.Name)
                .NotEmpty().WithMessage("Role name is required.")
                .Must(IsValidRoleName)
                .WithMessage("Roles must be defined in the application before being created.")
                .MustAsync(IsUniqueRoleName)
                .WithMessage("A role with this name already exists.");
        }

        private bool IsValidRoleName(string name)
        {
            if (name.Trim() != name) return false;

            return Enum.TryParse<Role>(name, true, out var roleEnumValue) && Enum.IsDefined(typeof(Role), roleEnumValue);
        }

        private async Task<bool> IsUniqueRoleName(string name, CancellationToken cancellationToken)
        {
            var role = await _roleManager.FindByNameAsync(name);
            return role == null;
        }
    }
}
