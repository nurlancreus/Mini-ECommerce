using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Mini_ECommerce.Application.Abstractions.Services.Application;
using Mini_ECommerce.Application.Attributes;
using Mini_ECommerce.Application.DTOs.Configuration;
using Mini_ECommerce.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Mini_ECommerce.Infrastructure.Concretes.Services.Application
{
    public class ApplicationService : IApplicationService
    {
        public List<MenuDTO> GetAuthorizeDefinitionEndpoints(Type type)
        {
            // Get the assembly of the provided type
            Assembly? assembly = Assembly.GetAssembly(type) ?? throw new ArgumentException("Invalid assembly type provided.");

            // Find all controllers within the assembly
            var controllers = assembly.GetTypes().Where(t => t.IsAssignableTo(typeof(ControllerBase)));

            var menus = new List<MenuDTO>();

            // Loop through each controller to find actions with AuthorizeDefinitionAttribute
            foreach (var controller in controllers)
            {
                var actions = controller.GetMethods()
                    .Where(m => m.IsDefined(typeof(AuthorizeDefinitionAttribute), inherit: true));

                foreach (var action in actions)
                {
                    // Retrieve the AuthorizeDefinitionAttribute
                    var authorizeDefinitionAttribute = action.GetCustomAttribute<AuthorizeDefinitionAttribute>();
                    if (authorizeDefinitionAttribute == null)
                        continue;

                    // Find or create the MenuDTO
                    var menu = menus.FirstOrDefault(m => m.Name == authorizeDefinitionAttribute.Menu.ToString());
                    if (menu == null)
                    {
                        menu = new MenuDTO
                        {
                            Name = authorizeDefinitionAttribute.Menu.ToString()
                        };

                        menus.Add(menu);
                    }

                    var roles = new string[authorizeDefinitionAttribute.Roles.Length];

                    for (int i = 0; i < roles.Length; i++)
                    {
                        var role = authorizeDefinitionAttribute.Roles[i].ToString();
                        roles[i] = role;
                    }

                    // Create ActionDTO based on attribute data
                    var actionDto = new ActionDTO
                    {
                        ActionType = authorizeDefinitionAttribute.ActionType.ToString(),
                        Definition = authorizeDefinitionAttribute.Definition,
                        Method = GetHttpMethod(action).ToString(),
                        Roles = roles
                    };

                    var code = new StringBuilder($"{actionDto.Method}.{menu.Name}.{actionDto.ActionType}.{FormatDefinition(actionDto.Definition)}");

                    if(actionDto.Roles.Length > 0)
                    {
                        code.Append($".{ string.Join(";", actionDto.Roles)}");
                    }

                    // Generate a unique code for the action
                    actionDto.Code = code.ToString();

                    menu.Actions.Add(actionDto);
                }
            }

            return menus;
        }

        // Helper method to determine HTTP method type
        private static MethodType GetHttpMethod(MethodInfo action)
        {
            var httpAttribute = action.GetCustomAttribute<HttpMethodAttribute>();
            if (httpAttribute != null && Enum.TryParse(httpAttribute.HttpMethods.First(), true, out MethodType method))
            {
                return method;
            }

            // Default to GET if no HTTP method attribute is defined
            return MethodType.GET;
        }

        // Helper method to capitalize each word in the string
        private static string FormatDefinition(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return string.Empty;

            // Split the input string by spaces
            var words = input.Split(' ', StringSplitOptions.RemoveEmptyEntries);

            // Capitalize each word using ToTitleCase
            var capitalizedWords = words.Select(word => CultureInfo.CurrentCulture.TextInfo.ToTitleCase(word.ToLower()));

            // Join the words back together without spaces
            return string.Join("", capitalizedWords).Replace("'", "");
        }
    }
}
