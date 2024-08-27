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

namespace Mini_ECommerce.Application.Features.Queries.Order.GetAllOrders
{
    public class GetAllOrdersQueryHandler : IRequestHandler<GetAllOrdersQueryRequest, GetAllOrdersQueryResponse>
    {
        private readonly IOrderService _orderService;

        public GetAllOrdersQueryHandler(IOrderService orderService)
        {
            _orderService = orderService;
        }

        public async Task<GetAllOrdersQueryResponse> Handle(GetAllOrdersQueryRequest request, CancellationToken cancellationToken)
        {
            var data = await _orderService.GetAllOrdersAsync(request.Page, request.PageSize);

            return new GetAllOrdersQueryResponse()
            {
                CurrentPage = data.Page,
                TotalOrderCount = data.TotalCount,
                TotalPagesCount = data.TotalPages,
                Orders = data.Orders.Select(o => new GetOrderVM
                {
                    CreatedAt = o.CreatedAt,
                    Description = o.Description,
                    Id = o.Id,
                    OrderCode = o.OrderCode,
                    TotalPrice = o.TotalPrice,

                    Address = new GetAddressVM()
                    {
                        Id = o.Address.Id,
                        Country = o.Address.Country,
                        City = o.Address.City,
                        State = o.Address.State,
                        Street = o.Address.Street,
                    },
                    BasketItems = o.BasketItems.Select(bi => new GetBasketItemVM()
                    {
                        Name = bi.Name,
                        Price = bi.Price,
                        Quantity = bi.Quantity,
                        TotalPrice = bi.TotalPrice,
                    }).ToList()
                }).ToList(),
            };
        }
    }
}
