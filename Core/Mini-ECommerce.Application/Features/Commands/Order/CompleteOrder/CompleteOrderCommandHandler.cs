using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mini_ECommerce.Application.Features.Commands.Order.CompleteOrder
{
    public class CompleteOrderCommandHandler : IRequestHandler<CompleteOrderCommandRequest, CompleteOrderCommandResponse>
    {
        public Task<CompleteOrderCommandResponse> Handle(CompleteOrderCommandRequest request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
