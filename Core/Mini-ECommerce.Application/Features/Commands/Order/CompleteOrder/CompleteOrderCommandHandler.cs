using MediatR;
using Mini_ECommerce.Application.Abstractions.Services;
using Mini_ECommerce.Application.DTOs.Customer;
using Mini_ECommerce.Application.DTOs.Order;
using Mini_ECommerce.Application.Exceptions;
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
        private readonly IMailService _mailService;

        public CompleteOrderCommandHandler(IOrderService orderService, IMailService mailService)
        {
            _orderService = orderService;
            _mailService = mailService;
        }

        public async Task<CompleteOrderCommandResponse> Handle(CompleteOrderCommandRequest request, CancellationToken cancellationToken)
        {
            (bool isSuccess, CompletedOrderDTO? completedOrder) = await _orderService.CompleteOrderAsync(request.Id);

            if (isSuccess && completedOrder != null)
            {
                var customer = new GetCustomerDTO()
                {
                    FirstName = completedOrder.Firstname,
                    LastName = completedOrder.Lastname,
                    UserName = completedOrder.Username,
                    Email = completedOrder.Email,
                };

                await _mailService.SendCompletedOrderMailAsync(completedOrder.OrderCode, completedOrder.OrderDate, customer);

                return new CompleteOrderCommandResponse()
                {
                    Success = isSuccess,
                    Message = "Order Completed Successfully!",
                    Order = new GetCompletedOrderVM()
                    {
                        OrderCode = completedOrder.OrderCode,
                        Firstname = completedOrder.Firstname,
                        Lastname = completedOrder.Lastname,
                        Username = completedOrder.Username,
                        Email = completedOrder.Email,
                        OrderDate = completedOrder.OrderDate,
                        CreatedAt = completedOrder.CreatedAt,
                    }
                };
            }
            else
            {
                throw new OrderNotCompletedException();
            }

        }
    }
}
