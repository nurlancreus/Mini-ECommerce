using MediatR;
using Mini_ECommerce.Application.ViewModels.Order;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mini_ECommerce.Application.Features.Commands.Order.CompleteOrder
{
    public class CompleteOrderCommandResponse : IRequest<CompleteOrderCommandRequest>
    {
        public bool Success { get; set; }   
        public string? Message { get; set; }
        public GetCompletedOrderVM Order {  get; set; }
    }
}
