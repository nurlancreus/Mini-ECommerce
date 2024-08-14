using Microsoft.Extensions.DependencyInjection;
using Mini_ECommerce.Application.Abstractions.Services;
using Mini_ECommerce.Infrastructure.Concretes.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mini_ECommerce.Infrastructure
{
    public static class ServiceRegistration
    {
        public static void AddInfrastructureServices(this IServiceCollection services)
        {
            services.AddScoped<IFileService, FileService>();
        }
    }
}
