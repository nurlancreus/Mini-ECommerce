using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Mini_ECommerce.Application.Abstractions.Repositories;
using Mini_ECommerce.Application.Features.Commands.Order.CompleteOrder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mini_ECommerce.Application.Validators.Order
{
    public class CompleteOrderCommandRequestValidator : AbstractValidator<CompleteOrderCommandRequest>
    {
        private readonly IOrderReadRepository _orderReadRepository;

        public CompleteOrderCommandRequestValidator(IOrderReadRepository orderReadRepository)
        {
            _orderReadRepository = orderReadRepository;
        }

        public CompleteOrderCommandRequestValidator()
        {
            RuleFor(co => co.Id).MustAsync(async (id, cancellation) =>
            {
                return await OrderExists(id);
            }).WithMessage("Order does not exist");
        }

        private async Task<bool> OrderExists(string id)
        {
            if (Guid.TryParse(id, out var parsedId))
            {
                return await _orderReadRepository.Table.AnyAsync(order => order.Id == parsedId);
            }
            return false;
        }
    }
}
