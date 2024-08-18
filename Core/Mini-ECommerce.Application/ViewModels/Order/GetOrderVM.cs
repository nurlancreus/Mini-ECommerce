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
        public Guid Id { get; set; }
        public string Description { get; set; }
        public string Address { get; set; }
        public string OrderCode { get; set; }
        public GetCustomerVM Customer { get; set; }
        public ICollection<GetProductVM> Products { get; set; } = [];
    }
}
