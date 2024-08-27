using Mini_ECommerce.Application.ViewModels.AppUser;
using Mini_ECommerce.Application.ViewModels.Order;
using Mini_ECommerce.Domain.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mini_ECommerce.Application.ViewModels.Customer
{
    public class GetCustomerVM
    {
        public string Id { get; set; }
        public GetAppUserVM AppUser { get; set; }
        public ICollection<GetOrderVM> Orders { get; set; } = [];
    }
}
