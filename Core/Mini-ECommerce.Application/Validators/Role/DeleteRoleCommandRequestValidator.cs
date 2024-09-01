using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Mini_ECommerce.Domain.Enums;
using Mini_ECommerce.Application.Features.Commands.Role.DeleteRole;
using Mini_ECommerce.Domain.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mini_ECommerce.Application.Validators
{
    public class DeleteRoleCommandRequestValidator : AbstractValidator<DeleteRoleCommandRequest>
    {
        private readonly RoleManager<AppRole> _roleManager;

        public DeleteRoleCommandRequestValidator(RoleManager<AppRole> roleManager)
        {
            _roleManager = roleManager;

            RuleFor(r => r.Id)
                .NotEmpty().WithMessage("Role Id is required.")
                .MustAsync(IsRoleExist)
                .WithMessage("A role with this Id is not exist.");
        }
        private async Task<bool> IsRoleExist(string id, CancellationToken cancellationToken)
        {
            var role = await _roleManager.FindByIdAsync(id);

            return role != null;
        }
    }
}
