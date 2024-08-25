using MediatR;
using Mini_ECommerce.Application.Abstractions.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mini_ECommerce.Application.Features.Commands.Basket.UpdateItemQuantity
{
    public class UpdateItemQuantityCommandHandler : IRequestHandler<UpdateItemQuantityCommandRequest, UpdateItemQuantityCommandResponse>
    {
        private readonly IBasketService _basketService;

        public UpdateItemQuantityCommandHandler(IBasketService basketService)
        {
            _basketService = basketService;
        }

        public async Task<UpdateItemQuantityCommandResponse> Handle(UpdateItemQuantityCommandRequest request, CancellationToken cancellationToken)
        {
            await _basketService.UpdateQuantityAsync(new()
            {
                BasketItemId = request.BasketItemId,
                Quantity = request.Quantity,
            });

            return new()
            {
                Success = true,
                Message = $"Item '{request.BasketItemId}' updated successfully!"
            };
        }
    }
}
