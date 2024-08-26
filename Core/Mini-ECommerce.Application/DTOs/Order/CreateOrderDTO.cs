using Mini_ECommerce.Application.DTOs.Address;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mini_ECommerce.Application.DTOs.Order
{
    public class CreateOrderDTO
    {
        public string? BasketId { get; set; }
        public string Description { get; set; }
        public GetAddressDTO Address { get; set; }
    }
}
