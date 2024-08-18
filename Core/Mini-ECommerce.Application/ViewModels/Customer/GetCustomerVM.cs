using Mini_ECommerce.Application.ViewModels.Order;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mini_ECommerce.Application.ViewModels.Customer
{
    public class GetCustomerVM
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public ICollection<GetOrderVM> Orders { get; set; } = [];
    }
}
