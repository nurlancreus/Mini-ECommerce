using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Mini_ECommerce.Application.Abstractions.Repositories;
using Mini_ECommerce.Application.Abstractions.Services;
using Mini_ECommerce.Application.Abstractions.Services.Application;
using Mini_ECommerce.Application.DTOs.AuthEndpoint;
using Mini_ECommerce.Application.DTOs.Role;
using Mini_ECommerce.Application.Exceptions;
using Mini_ECommerce.Application.Helpers;
using Mini_ECommerce.Domain.Entities;
using Mini_ECommerce.Domain.Entities.Identity;
using Mini_ECommerce.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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
                throw new ArgumentException("Invalid code format. Expected format 'Method.Menu.Action.Definition;...'");

            string methodType = codeParts[0];
            string menuName = codeParts[1];
            string actionType = codeParts[2];
            string definition = codeParts[3];

            if (!menuName.Equals(menu, StringComparison.CurrentCultureIgnoreCase))
                throw new InvalidOperationException($"Endpoint code '{code}' does not belong to menu '{menu}'");

            // Retrieve existing endpoint
            AppEndpoint? endpoint = await _appEndpointReadRepository.Table.Include(e => e.Roles).FirstOrDefaultAsync(e => e.Code == code && e.Menu == menuValue);

            if (endpoint != null)
            {
                if (roles.Length == 0)
                {
                    // If no roles are provided, remove all roles from the endpoint
                    endpoint.Roles.Clear();
                }
                else
                {
                    var rolesToBeRemoved = endpoint.Roles.Where(r => !roles.Contains(r.Name)).ToList();

                    // Remove roles that are not in the provided list
                    foreach (var role in rolesToBeRemoved)
                    {
                        endpoint.Roles.Remove(role);
                    }

                    // Add new roles if not already present
                    foreach (var roleName in roles)
                    {
                        if (!endpoint.Roles.Any(r => r.Name == roleName))
                        {
                            // Find role and add to endpoint if not present
                            AppRole? role = await _roleManager.FindByNameAsync(roleName) ?? throw new EntityNotFoundException(roleName);
                            endpoint.Roles.Add(role);
                        }
                    }
                }
            }
            else
            {
                // If endpoint does not exist, create a new one
                endpoint = new AppEndpoint();

                if (EnumHelpers.TryParseEnum(methodType, out MethodType parsedMethod) &&
                    EnumHelpers.TryParseEnum(actionType, out ActionType parsedAction))
                {
                    endpoint.Menu = menuValue;  // Use parsed menuValue here
                    endpoint.HttpMethod = parsedMethod;
                    endpoint.Action = parsedAction;
                    endpoint.Definition = definition;
                }
                else
                {
                    throw new InvalidOperationException("Values cannot be parsed to enum");
                }

                endpoint.Roles = new List<AppRole>();

                // Add roles to new endpoint
                foreach (var roleName in roles)
                {
                    AppRole? role = await _roleManager.FindByNameAsync(roleName) ?? throw new EntityNotFoundException(roleName);
                    endpoint.Roles.Add(role);
                }

                // Set code for new endpoint
                endpoint.Code = $"{methodType}.{menuName}.{actionType}.{definition}";

                await _appEndpointWriteRepository.AddAsync(endpoint);
            }

            // Save changes to the database
            await _appEndpointWriteRepository.SaveAsync();

            return endpoint.Code;
        }

        public async Task<GetAuthEndpointDTO> GetRolesToEndpointAsync(string code, string menu)
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

            return new GetAuthEndpointDTO()
            {
                Code = code,
                Roles = endpoint.Roles.Select(r => new GetRoleDTO()
                {
                    Id = r.Id,
                    Name = r.Name!
                }).ToList()
            };
        }
    }
}
