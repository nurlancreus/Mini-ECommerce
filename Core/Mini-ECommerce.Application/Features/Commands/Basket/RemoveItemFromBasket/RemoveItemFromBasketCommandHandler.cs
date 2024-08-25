using MediatR;
using Mini_ECommerce.Application.Abstractions.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mini_ECommerce.Application.Features.Commands.Basket.RemoveItemFromBasket
{
    public class RemoveItemFromBasketCommandHandler : IRequestHandler<RemoveItemFromBasketCommandRequest, RemoveItemFromBasketCommandResponse>
    {
        private readonly IBasketService _basketService;

        public RemoveItemFromBasketCommandHandler(IBasketService basketService)
        {
            _basketService = basketService;
        }

        public async Task<RemoveItemFromBasketCommandResponse> Handle(RemoveItemFromBasketCommandRequest request, CancellationToken cancellationToken)
        {

            await _basketService.RemoveBasketItemAsync(request.BasketItemId);

            return new()
            {
                Success = true,
                Message = $"Item '{request.BasketItemId}' removed from basket successfully!"
            };
        }
    }
}
