using MediatR;
using Mini_ECommerce.Application.ViewModels.Order;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mini_ECommerce.Application.Features.Queries.Order.GetOrderById
{
    public class GetOrderByIdQueryResponse : IRequest<GetOrderByIdQueryRequest>
    {
        public GetOrderVM Order {  get; set; }
    }
}
