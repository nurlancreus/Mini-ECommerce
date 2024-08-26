using MediatR;
using Mini_ECommerce.Application.Abstractions.Hubs;
using Mini_ECommerce.Application.Abstractions.Services;
using Mini_ECommerce.Application.DTOs.Address;
using Mini_ECommerce.Application.DTOs.Order;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mini_ECommerce.Application.Features.Commands.Order.CreateOrder
{
    public class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommandRequest, CreateOrderCommandResponse>
    {
        private readonly IOrderService _orderService;
        private readonly IBasketService _basketService;
        private readonly IOrderHubService _orderHubService;


        public CreateOrderCommandHandler(IOrderService orderService, IBasketService basketService, IOrderHubService orderHubService)
        {
            _orderService = orderService;
            _basketService = basketService;
            _orderHubService = orderHubService;
        }

        public async Task<CreateOrderCommandResponse> Handle(CreateOrderCommandRequest request, CancellationToken cancellationToken)
        {
            string? basketId = Convert.ToString(_basketService?.UserActiveBasket?.Id)?.ToString();

            await _orderService.CreateOrderAsync(new CreateOrderDTO()
            {
                BasketId = basketId,
                Description = request.Description,
                Address = new GetAddressDTO ()
                {
                    Country = request.Address.Country,
                    State = request.Address.State,
                    City = request.Address.City,
                    PostalCode = request.Address.PostalCode,
                    Street = request.Address.Street
                }
            });

            await _orderHubService.OrderAddedMessageAsync("New Order added!");

            return new ()
            {
                Success = true,
                Message = "Order added successfully!"
            };
        }
    }
}
