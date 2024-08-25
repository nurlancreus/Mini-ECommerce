using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mini_ECommerce.Application.DTOs.Basket
{
    public class CreateBasketItemDTO
    {
        // we could've demand BasketId from the client, but we won't. we will be add basketItem to the currently active basket. (which has no order yet)
        // we don't tell client you have such baskets and that is the active one etc.
        public string ProductId { get; set; }
        public int Quantity { get; set; }
    }
}
