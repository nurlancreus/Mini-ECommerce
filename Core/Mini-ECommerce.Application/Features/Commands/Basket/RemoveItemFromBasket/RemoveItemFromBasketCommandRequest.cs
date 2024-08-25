using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mini_ECommerce.Application.Features.Commands.Basket.RemoveItemFromBasket
{
    public class RemoveItemFromBasketCommandRequest : IRequest<RemoveItemFromBasketCommandResponse>
    {
        public string BasketItemId { get; set; }
    }
}
