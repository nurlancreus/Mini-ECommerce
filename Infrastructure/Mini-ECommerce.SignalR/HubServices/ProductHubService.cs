using Microsoft.AspNetCore.SignalR;
using Mini_ECommerce.Application.Abstractions.Hubs;
using Mini_ECommerce.SignalR.Constants;
using Mini_ECommerce.SignalR.Hubs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mini_ECommerce.SignalR.HubServices
{
    public class ProductHubService : IProductHubService
    {
        readonly IHubContext<ProductHub> _hubContext;

        public ProductHubService(IHubContext<ProductHub> hubContext)
        {
            _hubContext = hubContext;
        }

        public async Task ProductAddedMessageAsync(string message)
        {
            await _hubContext.Clients.All.SendAsync(FunctionNames.ProductAddedMessage, message);
        }
    }
}
