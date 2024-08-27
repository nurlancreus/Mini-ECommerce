using Mini_ECommerce.Application.DTOs.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mini_ECommerce.Application.DTOs.Customer
{
    public class GetCustomerDTO
    {
        string Id { get; set; }
        public GetAppUserDTO AppUser { get; set; }
    }
}
