using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mini_ECommerce.Application.DTOs.Basket
{
    public class GetBasketItemDTO
    {
        public string Name { get; set; }
        public float Price { get; set; }
        public int Quantity { get; set; }
        public float TotalPrice {  get; set; }
    }
}
