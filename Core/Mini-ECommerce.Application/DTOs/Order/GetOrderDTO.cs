using Mini_ECommerce.Application.DTOs.Address;
using Mini_ECommerce.Application.DTOs.Basket;
using Mini_ECommerce.Application.DTOs.Customer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mini_ECommerce.Application.DTOs.Order
{
    public class GetOrderDTO
    {
        public string Id { get; set; }
        public GetAddressDTO Address { get; set; }
        public GetCustomerDTO Customer { get; set; }
        public List<GetBasketItemDTO> BasketItems { get; set; } = [];
        public DateTime CreatedAt { get; set; }
        public string Description { get; set; }
        public string OrderCode { get; set; }
        public bool Completed { get; set; }
        public float TotalPrice { get; set; }
        
    }
}
