using MediatR;
using Mini_ECommerce.Application.Abstractions.Services;
using Mini_ECommerce.Application.DTOs.Basket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mini_ECommerce.Application.Features.Queries.Basket.GetBasketItems
{
    public class GetBasketItemsQueryHandler : IRequestHandler<GetBasketItemsQueryRequest, GetBasketItemsQueryResponse>
    {
        private readonly IBasketService _basketService;

        public GetBasketItemsQueryHandler(IBasketService basketService)
        {
            _basketService = basketService;
        }

        public async Task<GetBasketItemsQueryResponse> Handle(GetBasketItemsQueryRequest request, CancellationToken cancellationToken)
        {
            var basketItems = await _basketService.GetBasketItemsAsync();

            return new GetBasketItemsQueryResponse()
            {
                BasketItems = basketItems.Select(item => new GetBasketItemDTO()
                {
                    Name = item.Product.Name,
                    Price = item.Product.Price,
                    Quantity = item.Quantity,
                }).ToList()
            };
        }
    }
}
