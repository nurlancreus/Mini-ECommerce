using MediatR;
using Mini_ECommerce.Application.Abstractions.Services;
using Mini_ECommerce.Application.DTOs.Order;
using Mini_ECommerce.Application.ViewModels.Order;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mini_ECommerce.Application.Features.Commands.Order.CompleteOrder
{
    public class CompleteOrderCommandHandler : IRequestHandler<CompleteOrderCommandRequest, CompleteOrderCommandResponse>
    {
        private readonly IOrderService _orderService;

        public CompleteOrderCommandHandler(IOrderService orderService)
        {
            _orderService = orderService;
        }

        public async Task<CompleteOrderCommandResponse> Handle(CompleteOrderCommandRequest request, CancellationToken cancellationToken)
        {
            (bool isSuccess, CompletedOrderDTO? completedOrder) = await _orderService.CompleteOrderAsync(request.Id);

            return new CompleteOrderCommandResponse()
            {
                Success = isSuccess,
                Message = "Order Completed Successfully!",
                Order = new GetCompletedOrderVM()
                {
                    OrderCode = completedOrder.OrderCode,
                    Username = completedOrder.Username,
                    Email = completedOrder.Email,
                    OrderDate = completedOrder.OrderDate,
                }
            };
        }
    }
}
