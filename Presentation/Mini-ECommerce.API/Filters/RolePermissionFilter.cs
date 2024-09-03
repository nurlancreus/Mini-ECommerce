using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.Extensions.Logging;
using Mini_ECommerce.Application.Abstractions.Services;
using Mini_ECommerce.Application.Attributes;
using Mini_ECommerce.Domain.Enums;
using Mini_ECommerce.Infrastructure.Concretes.Services.Application;
using System.Reflection;

namespace ETicaretAPI.API.Filters
{
    public class RolePermissionFilter : IAsyncActionFilter
    {
        private readonly IUserService _userService;
        private readonly ILogger<RolePermissionFilter> _logger;
        private const string AdminUser = "nurlancreus"; // Constant for admin username

        public RolePermissionFilter(IUserService userService, ILogger<RolePermissionFilter> logger)
        {
            _userService = userService;
            _logger = logger;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var name = context.HttpContext.User.Identity?.Name;

            // Check if the user is authenticated
            if (string.IsNullOrEmpty(name))
            {
                _logger.LogWarning("User is not authenticated. Redirecting to login.");
                context.Result = new UnauthorizedResult(); 
                return;
            }

            // Check if the user is an admin
            if (name == AdminUser)
            {
                _logger.LogInformation("User '{UserName}' is an admin. Proceeding to next action.", name);
                await next();
                return;
            }

            if (context.ActionDescriptor is not ControllerActionDescriptor descriptor)
            {
                _logger.LogWarning("ControllerActionDescriptor is null. Proceeding to next action.");
                await next();
                return;
            }

            var attribute = descriptor.MethodInfo.GetCustomAttribute<AuthorizeDefinitionAttribute>();

            if (attribute == null)
            {
                _logger.LogWarning("AuthorizeDefinitionAttribute is not found for action '{ActionName}'. Proceeding to next action.", descriptor.ActionName);
                await next();
                return;
            }

            var httpAttribute = descriptor.MethodInfo.GetCustomAttribute<HttpMethodAttribute>();
            var httpMethod = httpAttribute?.HttpMethods.FirstOrDefault() ?? MethodType.GET.ToString();

            var code = $"{httpMethod}.{attribute.Menu}.{attribute.ActionType}.{ApplicationService.FormatDefinition(attribute.Definition)}";

            try
            {
                _logger.LogInformation("Checking role permissions for user '{UserName}' on endpoint '{EndpointCode}'.", name, code);
                var hasRole = await _userService.HasRolePermissionToEndpointAsync(name, code);

                if (!hasRole)
                {
                    _logger.LogWarning("User '{UserName}' does not have permission for endpoint '{EndpointCode}'.", name, code);
                    context.Result = new UnauthorizedResult();
                    return;
                }

                _logger.LogInformation("User '{UserName}' has permission for endpoint '{EndpointCode}'.", name, code);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while checking permissions for user '{UserName}' on endpoint '{EndpointCode}'.", name, code);
                context.Result = new StatusCodeResult(StatusCodes.Status500InternalServerError);
                return;
            }

            await next();
        }
    }
}
