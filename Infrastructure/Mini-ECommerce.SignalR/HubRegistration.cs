using Microsoft.AspNetCore.Builder;
using Mini_ECommerce.SignalR.Hubs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mini_ECommerce.SignalR
{
    public static class HubRegistration
    {
        public static void MapHubs(this WebApplication webApplication)
        {
            webApplication.MapHub<ProductHub>("/products-hub");
            // webApplication.MapHub<OrderHub>("/orders-hub");
        }
    }
}
