using MediatR;
using Mini_ECommerce.Application.Abstractions.Services;
using Mini_ECommerce.Application.ViewModels.Address;
using Mini_ECommerce.Application.ViewModels.Basket;
using Mini_ECommerce.Application.ViewModels.Order;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mini_ECommerce.Application.Features.Queries.Order.GetOrderById
{
    public class GetOrderByIdQueryHandler : IRequestHandler<GetOrderByIdQueryRequest, GetOrderByIdQueryResponse>
    {
        private readonly IOrderService _orderService;

        public GetOrderByIdQueryHandler(IOrderService orderService)
        {
            _orderService = orderService;
        }

        public async Task<GetOrderByIdQueryResponse> Handle(GetOrderByIdQueryRequest request, CancellationToken cancellationToken)
        {
            var order = await _orderService.GetOrderByIdAsync(request.Id);

            return new GetOrderByIdQueryResponse()
            {
                Order = new GetOrderVM()
                {
                    Id = order.Id,
                    CreatedAt = order.CreatedAt,
                    Description = order.Description,
                    OrderCode = order.OrderCode,
                    TotalPrice = order.TotalPrice,
                    Address = new GetAddressVM()
                    {
                        Id = order.Address.Id,
                        Country = order.Address.Country,
                        City = order.Address.City,
                        State = order.Address.State,
                        Street = order.Address.Street,
                    },
                    BasketItems = order.BasketItems.Select(bi => new GetBasketItemVM()
                    {
                        Name = bi.Name,
                        Price = bi.Price,
                        Quantity = bi.Quantity,
                        TotalPrice = bi.TotalPrice,
                    }).ToList()
                }
            };
        }
    }
}
