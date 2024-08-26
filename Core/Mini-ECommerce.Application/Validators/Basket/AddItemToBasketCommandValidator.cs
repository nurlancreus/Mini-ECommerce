using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Mini_ECommerce.Application.Abstractions.Repositories;
using Mini_ECommerce.Application.Features.Commands.Basket.AddItemToBasket;
using System;
using System.Threading.Tasks;

namespace Mini_ECommerce.Application.Validators.Basket
{
    public class AddItemToBasketCommandValidator : AbstractValidator<AddItemToBasketCommandRequest>
    {
        private readonly IProductReadRepository _productReadRepository;

        public AddItemToBasketCommandValidator(IBasketItemReadRepository basketItemReadRepository, IProductReadRepository productReadRepository)
        {
            _productReadRepository = productReadRepository;

            RuleFor(bi => bi.ProductId)
                .NotEmpty()
                    .WithMessage("Please do not leave the product Id empty.")
                .NotNull()
                    .WithMessage("Product Id is required.")
                .MustAsync(async (id, cancellation) =>
                {
                    return await ProductExists(id);
                }).WithMessage("Product does not exist");

            RuleFor(bi => bi.Quantity)
                .NotNull()
                    .WithMessage("Quantity is required.")
                .GreaterThanOrEqualTo(0)
                    .WithMessage("Quantity cannot be negative.")
                .MustAsync(async (bi, quantity, cancellation) =>
                {
                    return await HasSufficientStock(bi.ProductId, quantity);
                }).WithMessage("Insufficient stock for the requested quantity.");
        }

        private async Task<bool> ProductExists(string productId)
        {
            if (Guid.TryParse(productId, out var parsedId))
            {
                return await _productReadRepository.Table.AnyAsync(product => product.Id == parsedId);
            }
            return false;
        }

        private async Task<bool> HasSufficientStock(string productId, int quantity)
        {
            if (Guid.TryParse(productId, out var parsedId))
            {
                var product = await _productReadRepository.Table
                    .FirstOrDefaultAsync(product => product.Id == parsedId);

                if (product != null)
                {
                    return product.Stock >= quantity;
                }
            }
            return false;
        }
    }
}
