using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Mini_ECommerce.Application.Abstractions.Repositories;
using Mini_ECommerce.Application.Features.Commands.ProductImageFile.ChangeMainImage;
using Mini_ECommerce.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mini_ECommerce.Application.Validators.ProductImageFile
{
    public class ChangeMainImageCommandRequestValidator : AbstractValidator<ChangeMainImageCommandRequest>
    {
        private readonly IProductReadRepository _productReadRepository;

        public ChangeMainImageCommandRequestValidator(IProductReadRepository productReadRepository)
        {
            _productReadRepository = productReadRepository;

            RuleFor(p => p.ProductId)
                .NotNull()
                .NotEmpty()
                .WithMessage("Product Id must be defined before.")
                .MustAsync(ProductExist)
                .WithMessage("Product with that Id is not exist");
            RuleFor(p => p.ProductImageId)
                .NotNull()
                .NotEmpty()
                .WithMessage("Product Image Id must be defined before.")
                .MustAsync(ProductImageExist)
                .WithMessage("Product Image with that Id is not exist in product images");
        }

        private async Task<bool> ProductExist(string id, CancellationToken cancellationToken)
        {
            var product = await _productReadRepository.Table
                .FirstOrDefaultAsync(p => p.Id.ToString() == id, cancellationToken);

            return product != null;
        }

        private async Task<bool> ProductImageExist(ChangeMainImageCommandRequest request, string productImageId, CancellationToken cancellationToken)
        {
            var product = await _productReadRepository.Table.Include(p => p.ProductImageFiles).FirstOrDefaultAsync(p => p.Id.ToString() == request.ProductId, cancellationToken);

            var productImage = product?.ProductImageFiles.FirstOrDefault(i => i.Id.ToString() == request.ProductImageId);

            return product != null && productImage != null;
        }
    }
}
