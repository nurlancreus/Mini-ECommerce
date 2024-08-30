using MediatR;
using Mini_ECommerce.Application.ViewModels.Customer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mini_ECommerce.Application.Features.Queries.Order.GetAllCustomers
{
    public class GetAllCustomersQueryResponse : IRequest<GetAllCustomersQueryRequest>
    {
        public List<GetCustomerVM> Customers { get; set; } = [];
    }
}
