using Mini_ECommerce.Application.DTOs.Basket;
using Mini_ECommerce.Application.ViewModels.Address;
using Mini_ECommerce.Application.ViewModels.Basket;
using Mini_ECommerce.Application.ViewModels.Customer;
using Mini_ECommerce.Application.ViewModels.Product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mini_ECommerce.Application.ViewModels.Order
{
    public class GetOrderVM
    {
        public string Id { get; set; }
        public string Description { get; set; }
        public GetAddressVM Address { get; set; }
        public string OrderCode { get; set; }
        public float TotalPrice { get; set; }
        public bool IsCompleted { get; set; }
        public GetCustomerVM Customer { get; set; }
        public DateTime CreatedAt { get; set; }
        public List<GetBasketItemVM> BasketItems { get; set; } = [];
    }
}
