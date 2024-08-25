using MediatR;
using Mini_ECommerce.Application.DTOs.Basket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mini_ECommerce.Application.Features.Queries.Basket.GetBasketItems
{
    public class GetBasketItemsQueryResponse : IRequest<GetBasketItemsQueryRequest>
    {
        public ICollection<GetBasketItemDTO> BasketItems { get; set; } = [];
    }
}
