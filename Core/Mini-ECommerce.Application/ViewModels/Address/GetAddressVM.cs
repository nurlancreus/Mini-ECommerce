using Mini_ECommerce.Application.ViewModels.Order;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mini_ECommerce.Application.ViewModels.Address
{
    public class GetAddressVM
    {
        public Guid Id { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        // public string PostalCode { get; set; }
        public string Country { get; set; }
        public ICollection<GetOrderVM> Orders { get; set; }
    }
}
