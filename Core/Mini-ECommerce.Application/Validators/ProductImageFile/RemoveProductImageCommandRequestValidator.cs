using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Mini_ECommerce.Application.Abstractions.Repositories;
using Mini_ECommerce.Application.Features.Commands.ProductImageFile.RemoveProductImage;
using System.Threading;
using System.Threading.Tasks;

namespace Mini_ECommerce.Application.Validators.ProductImageFile
{
    public class RemoveProductImageCommandRequestValidator : AbstractValidator<RemoveProductImageCommandRequest>
    {
        private readonly IProductReadRepository _productReadRepository;

        public RemoveProductImageCommandRequestValidator(IProductReadRepository productReadRepository)
        {
            _productReadRepository = productReadRepository;

            RuleFor(p => p.ProductId)
                .NotNull()
                .NotEmpty()
                .WithMessage("Product Id must be provided.")
                .MustAsync(ProductExists)
                .WithMessage("Product with the given Id does not exist.");

            RuleFor(p => p.ProductImageId)
                .NotNull()
                .NotEmpty()
                .WithMessage("Product Image Id must be provided.")
                .MustAsync(ProductImageExists)
                .WithMessage("Product Image with the given Id does not exist in product images.");
        }

        private async Task<bool> ProductExists(string productId, CancellationToken cancellationToken)
        {
            var product = await _productReadRepository.Table
                .FirstOrDefaultAsync(p => p.Id.ToString() == productId, cancellationToken);
            return product != null;
        }

        private async Task<bool> ProductImageExists(RemoveProductImageCommandRequest request, string productImageId, CancellationToken cancellationToken)
        {
            var product = await _productReadRepository.Table
                .Include(p => p.ProductImageFiles) 
                .FirstOrDefaultAsync(p => p.Id.ToString() == request.ProductId, cancellationToken);

            var productImage = product?.ProductImageFiles.FirstOrDefault(i => i.Id.ToString() == productImageId);

            return product != null && productImage != null;
        }
    }
}
