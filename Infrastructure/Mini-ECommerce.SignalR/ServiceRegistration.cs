using Microsoft.Extensions.DependencyInjection;
using Mini_ECommerce.Application.Abstractions.Hubs;
using Mini_ECommerce.SignalR.HubServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mini_ECommerce.SignalR
{
    public static class ServiceRegistration
    {
        public static IServiceCollection AddSignalRServices(this IServiceCollection services)
        {
            services.AddTransient<IProductHubService, ProductHubService>();
            services.AddTransient<IOrderHubService, OrderHubService>();
            services.AddSignalR();

            return services;
        }
    }
}
