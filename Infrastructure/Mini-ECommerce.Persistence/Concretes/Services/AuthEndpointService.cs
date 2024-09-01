using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Mini_ECommerce.Application.Abstractions.Repositories;
using Mini_ECommerce.Application.Abstractions.Services;
using Mini_ECommerce.Application.Abstractions.Services.Application;
using Mini_ECommerce.Application.DTOs.Role;
using Mini_ECommerce.Application.Exceptions;
using Mini_ECommerce.Application.Helpers;
using Mini_ECommerce.Domain.Entities;
using Mini_ECommerce.Domain.Entities.Identity;
using Mini_ECommerce.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mini_ECommerce.Persistence.Concretes.Services
{
    public class AuthEndpointService : IAuthEndpointService
    {
        private readonly IApplicationService _applicationService;
        private readonly RoleManager<AppRole> _roleManager;
        private readonly IAppEndpointReadRepository _appEndpointReadRepository;
        private readonly IAppEndpointWriteRepository _appEndpointWriteRepository;

        public AuthEndpointService(IApplicationService applicationService, RoleManager<AppRole> roleManager, IAppEndpointReadRepository appEndpointReadRepository, IAppEndpointWriteRepository appEndpointWriteRepository)
        {
            _applicationService = applicationService;
            _roleManager = roleManager;
            _appEndpointReadRepository = appEndpointReadRepository;
            _appEndpointWriteRepository = appEndpointWriteRepository;
        }

        public async Task<string> AssignRoleEndpointAsync(string[] roles, string menu, string code, Type type)
        {
            if (!EnumHelpers.TryParseEnum(menu, out AuthorizedMenu menuValue))
            {
                throw new InvalidOperationException("Value cannot be parsed to enum");
            }

            // Parse code into its parts
            string[] codeParts = code.Split(".");
            if (codeParts.Length < 4)
                throw new ArgumentException("Invalid code format. Expected format 'Method.Menu.Action.Definition.Role1;Role2;...'");

            string methodType = codeParts[0];
            string menuName = codeParts[1];
            string actionType = codeParts[2];
            string definition = codeParts[3];

            if (!menuName.Equals(menu, StringComparison.CurrentCultureIgnoreCase)) throw new InvalidOperationException($"Endpoint code '{code}' is not belongs to menu '{menu}'");

            // Retrieve existing endpoint
            AppEndpoint? endpoint = await _appEndpointReadRepository.GetSingleAsync(e => e.Code == code && e.Menu == menuValue);  

            // Check if roles part is present in the code, handle null gracefully
            List<string> endPointDefinedRoles = codeParts.Length > 4 ? [.. codeParts[4].Split(";")] : [];

            if (endpoint != null)
            {
                if (roles.Length == 0)
                {
                    // If no roles are provided, remove all roles from the endpoint
                    endpoint.Roles.Clear();
                    endPointDefinedRoles.Clear();
                }
                else
                {
                    // Update endpoint roles to match provided roles
                    foreach (var roleName in roles)
                    {
                        if (!endPointDefinedRoles.Contains(roleName))
                        {
                            endPointDefinedRoles.Add(roleName); // Add missing roles to the endpoint-defined roles
                        }

                        if (!endpoint.Roles.Any(r => r.Name == roleName))
                        {
                            // Find role and add to endpoint if not present
                            AppRole? role = await _roleManager.FindByNameAsync(roleName) ?? throw new EntityNotFoundException(roleName);
                            endpoint.Roles.Add(role);
                        }
                    }

                    // Remove roles from endpoint that are not in the provided roles list
                    foreach (var roleName in endPointDefinedRoles.ToList()) // Clone list to avoid modifying while iterating
                    {
                        if (!roles.Contains(roleName))
                        {
                            var roleToRemove = endpoint.Roles.FirstOrDefault(r => r.Name == roleName);
                            if (roleToRemove != null)
                            {
                                endpoint.Roles.Remove(roleToRemove);
                            }

                            endPointDefinedRoles.Remove(roleName);
                        }
                    }
                }

                // Update the code with the synchronized roles or empty if no roles
                string newCode = $"{methodType}.{menuName}.{actionType}.{definition}";
                if (endPointDefinedRoles.Count > 0)
                {
                    newCode += $".{string.Join(";", endPointDefinedRoles)}";
                }

                endpoint.Code = newCode;
            }
            else
            {
                // If endpoint does not exist, create a new one
                endpoint = new AppEndpoint();

                if (Enum.TryParse<MethodType>(methodType, out var parsedMethod) &&
                    Enum.TryParse<AuthorizedMenu>(menu, out var parsedMenu) &&
                    Enum.TryParse<ActionType>(actionType, out var parsedAction))
                {
                    endpoint.Menu = parsedMenu;
                    endpoint.HttpMethod = parsedMethod;
                    endpoint.Action = parsedAction;
                }
                else
                {
                    throw new InvalidOperationException("Values cannot be parsed to enum");
                }

                endpoint.Roles = [];

                // Add roles to new endpoint
                foreach (var roleName in roles)
                {
                    AppRole? role = await _roleManager.FindByNameAsync(roleName) ?? throw new EntityNotFoundException(roleName);
                    endpoint.Roles.Add(role);
                }

                // Set code for new endpoint, include roles if provided
                endpoint.Code = roles.Length > 0
                    ? $"{methodType}.{menuName}.{actionType}.{definition}.{string.Join(";", roles)}"
                    : $"{methodType}.{menuName}.{actionType}.{definition}";

                await _appEndpointWriteRepository.AddAsync(endpoint);
            }

            // Save changes to the database
            await _appEndpointWriteRepository.SaveAsync();

            return endpoint.Code;
        }

        public async Task<List<GetRoleDTO>> GetRolesToEndpointAsync(string code, string menu)
        {
            if (!EnumHelpers.TryParseEnum(menu, out AuthorizedMenu menuValue))
            {
                throw new InvalidOperationException("Value cannot be parsed to enum");
            }

            var endpoint = await _appEndpointReadRepository.Table
                .Include(e => e.Roles)
                .FirstOrDefaultAsync(e => e.Code == code && e.Menu == menuValue);

            if (endpoint == null)
            {
                throw new EntityNotFoundException(nameof(endpoint));
            }

            return endpoint.Roles.Select(r => new GetRoleDTO
            {
                Name = r.Name!,
                Id = r.Id
            }).ToList();
        }
    }
}
