using Mini_ECommerce.Application.DTOs.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mini_ECommerce.Application.Abstractions.Services.Application
{
    public interface IApplicationService
    {
        List<MenuDTO> GetAuthorizeDefinitionEndpoints(Type type);
    }
}
