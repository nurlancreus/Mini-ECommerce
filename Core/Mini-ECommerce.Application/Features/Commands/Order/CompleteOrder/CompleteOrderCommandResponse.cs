using MediatR;
using Mini_ECommerce.Application.Responses;
using Mini_ECommerce.Application.ViewModels.Order;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mini_ECommerce.Application.Features.Commands.Order.CompleteOrder
{
    public class CompleteOrderCommandResponse : BaseResponse, IRequest<CompleteOrderCommandRequest>
    {
        public GetCompletedOrderVM Order { get; set; }
    }
}
