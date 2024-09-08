using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Mini_ECommerce.Application.Features.Commands.User.AssignRoleToUser;
using Mini_ECommerce.Application.Helpers;
using Mini_ECommerce.Domain.Entities.Identity;
using Mini_ECommerce.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mini_ECommerce.Application.Validators.User
{
    public class AssignRoleToUserCommandRequestValidator : AbstractValidator<AssignRoleToUserCommandRequest>
    {
        private readonly RoleManager<AppRole> _roleManager;
        public AssignRoleToUserCommandRequestValidator(RoleManager<AppRole> roleManager)
        {
            _roleManager = roleManager;

            RuleFor(x => x.Id)
             .NotEmpty()
             .NotNull()
             .WithMessage("User Id is required.");

            RuleFor(x => x.Roles)
                .MustAsync(ContainValidRolesAsync)
                .WithMessage("Roles list contains invalid roles.");
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
    }
}
