using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mini_ECommerce.Application.Features.Queries.Order.GetAllCustomers
{
    public class GetAllCustomersQueryRequest : IRequest<GetAllCustomersQueryResponse>
    {
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 5;
    }
}
