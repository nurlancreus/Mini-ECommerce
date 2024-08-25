using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mini_ECommerce.Application.DTOs.Basket
{
    public class UpdateBasketItemDTO
    {
        public string BasketItemId { get; set; }
        public int Quantity { get; set; }
    }
}
