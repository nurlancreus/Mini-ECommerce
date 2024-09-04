using FluentValidation;
using Mini_ECommerce.Application.Abstractions.Repositories;
using Mini_ECommerce.Application.Features.Commands.Product.UpdateStockQrCodeToProduct;
using System.Threading.Tasks;

namespace Mini_ECommerce.Application.Validators.Product
{
    public class UpdateStockQrCodeToProductCommandRequestValidator : AbstractValidator<UpdateStockQrCodeToProductCommandRequest>
    {
        private readonly IProductReadRepository _productReadRepository;

        public UpdateStockQrCodeToProductCommandRequestValidator(IProductReadRepository productReadRepository)
        {
            _productReadRepository = productReadRepository;

            RuleFor(p => p.Id)
                .MustAsync(ProductExists).WithMessage("Product with the given ID does not exist.");

            RuleFor(p => p.Stock)
                .GreaterThan(0).WithMessage("Stock count must be a positive number.");
        }

        private async Task<bool> ProductExists(string productId, CancellationToken cancellationToken)
        {
            var product = await _productReadRepository.GetByIdAsync(productId, false);
            return product != null;
        }
    }
}
