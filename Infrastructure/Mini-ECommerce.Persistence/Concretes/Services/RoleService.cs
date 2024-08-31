using Microsoft.AspNetCore.Identity;
using Mini_ECommerce.Application.Abstractions.Services;
using Mini_ECommerce.Application.DTOs.Pagination;
using Mini_ECommerce.Application.DTOs.Role;
using Mini_ECommerce.Application.Enums;
using Mini_ECommerce.Application.Exceptions;
using Mini_ECommerce.Domain.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Mini_ECommerce.Persistence.Concretes.Services
{
    public class RoleService : IRoleService
    {
        private readonly RoleManager<AppRole> _roleManager;
        private readonly IPaginationService _paginationService;

        public RoleService(RoleManager<AppRole> roleManager, IPaginationService paginationService)
        {
            _roleManager = roleManager;
            _paginationService = paginationService;
        }

        public async Task CreateRoleAsync(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentException("Role name cannot be null or empty.", nameof(name));
            }

            // Check if the role name is defined in the Role enum
            if (!Enum.TryParse<Role>(name.Trim(), true, out var roleEnumValue) || !Enum.IsDefined(typeof(Role), roleEnumValue))
            {
                throw new RoleException("Roles must be defined in the application before being created.");
            }

            // Check if the role already exists in the database
            var existingRole = await _roleManager.FindByNameAsync(name);

            if (existingRole != null)
            {
                throw new RoleException($"Role '{name}' already exists.");
            }

            var role = new AppRole { Name = name };

            // Create the role in the identity system
            IdentityResult result = await _roleManager.CreateAsync(role);

            if (!result.Succeeded)
            {
                throw new RoleException(GetErrorMessage(result.Errors));
            }
        }

        public async Task DeleteRoleAsync(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                throw new ArgumentException("Role ID cannot be null or empty.", nameof(id));
            }

            AppRole? role = await _roleManager.FindByIdAsync(id);

            if (role == null)
            {
                throw new EntityNotFoundException(nameof(role), id);
            }

            IdentityResult result = await _roleManager.DeleteAsync(role);

            if (!result.Succeeded)
            {

                throw new RoleException(GetErrorMessage(result.Errors));

            }
        }

        public async Task<GetAllRolesDTO> GetAllRolesAsync(int page, int size)
        {
            var query = _roleManager.Roles;

            var paginationRequest = new PaginationRequestDTO()
            {
                Page = page,
                PageSize = size
            };

            var (count, _, _, _, paginatedQuery) = await _paginationService.ConfigurePaginationAsync(paginationRequest, query);


            return new GetAllRolesDTO()
            {
                Count = count,
                Roles = [.. query.Select(r => new GetRoleDTO ()
                {
                    Id = r.Id,
                    Name = r.Name!
                })],
            };
        }

        public async Task<GetRoleDTO> GetRoleByIdAsync(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                throw new ArgumentException("Role ID cannot be null or empty.", nameof(id));
            }

            var role = await _roleManager.FindByIdAsync(id);

            if (role == null)
            {
                throw new EntityNotFoundException(nameof(role), id);
            }

            return new GetRoleDTO
            {
                Id = role.Id,
                Name = role.Name!
            };
        }

        public async Task UpdateRoleAsync(string id, string name)
        {
            if (string.IsNullOrWhiteSpace(id) || string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("Role ID and name cannot be null or empty.");
            }

            var role = await _roleManager.FindByIdAsync(id);

            if (role == null)
            {
                throw new EntityNotFoundException(nameof(role), id);
            }

            role.Name = name;
            IdentityResult result = await _roleManager.UpdateAsync(role);

            if (!result.Succeeded)
            {

                throw new RoleException(GetErrorMessage(result.Errors));
            }
        }

        private static string GetErrorMessage(IEnumerable<IdentityError> identityErrors)
        {
            var errors = identityErrors.Select(e => e.Description);

            return string.Join(";", errors);
        }
    }
}
