using MediatR;
using Mini_ECommerce.Application.ViewModels.Address;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mini_ECommerce.Application.Features.Commands.Order.CreateOrder
{
    public class CreateOrderCommandRequest : IRequest<CreateOrderCommandResponse>
    {
        public string Description { get; set; }
        public CreateAddressVM Address {  get; set; }
    }
}
