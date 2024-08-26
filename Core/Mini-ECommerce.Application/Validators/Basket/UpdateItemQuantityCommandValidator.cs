using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Mini_ECommerce.Application.Abstractions.Repositories;
using Mini_ECommerce.Application.Features.Commands.Basket.UpdateItemQuantity;
using System;
using System.Threading.Tasks;

namespace Mini_ECommerce.Application.Validators.Basket
{
    public class UpdateItemQuantityCommandValidator : AbstractValidator<UpdateItemQuantityCommandRequest>
    {
        private readonly IBasketItemReadRepository _basketItemReadRepository;

        public UpdateItemQuantityCommandValidator(IBasketItemReadRepository basketItemReadRepository)
        {
            _basketItemReadRepository = basketItemReadRepository;

            RuleFor(bi => bi.BasketItemId)
                .NotEmpty()
                    .WithMessage("Please do not leave the basket item Id empty.")
                .NotNull()
                    .WithMessage("Basket item Id is required.")
                .MustAsync(async (id, cancellation) =>
                {
                    return await BasketItemExists(id);
                }).WithMessage("Basket item does not exist");

            RuleFor(bi => bi.Quantity)
                .GreaterThanOrEqualTo(0)
                    .WithMessage("Quantity cannot be negative.")
                .MustAsync(async (bi, quantity, cancellation) =>
                {
                    return await HasSufficientStock(bi.BasketItemId, quantity);
                }).WithMessage("Insufficient stock for the requested quantity.");
        }

        private async Task<bool> BasketItemExists(string basketItemId)
        {
            if (Guid.TryParse(basketItemId, out var parsedId))
            {
                return await _basketItemReadRepository.Table.AnyAsync(basketItem => basketItem.Id == parsedId);
            }
            return false;
        }

        private async Task<bool> HasSufficientStock(string basketItemId, int quantity)
        {
            if (Guid.TryParse(basketItemId, out var parsedBasketItemId))
            {
                // Get the basket item to retrieve associated product Id and current quantity
                var basketItem = await _basketItemReadRepository.Table
                    .Include(bi => bi.Product) // Assuming there is a navigation property to Product
                    .FirstOrDefaultAsync(basketItem => basketItem.Id == parsedBasketItemId);

                if (basketItem != null && basketItem.Product != null)
                {
                    // Get the product and check if there is sufficient stock
                    var product = basketItem.Product;
                    var currentStock = product.Stock;

                    // Assuming basketItem.Quantity is the quantity before the update
                    return (currentStock + basketItem.Quantity) >= quantity;
                }
            }
            return false;
        }
    }
}
