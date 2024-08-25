using MediatR;
using Mini_ECommerce.Application.Abstractions.Repositories;
using Mini_ECommerce.Application.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using static System.Net.Mime.MediaTypeNames;

namespace Mini_ECommerce.Application.Features.Commands.ProductImageFile.UpdateProductImage
{
    public class UpdateProductImageCommandHandler : IRequestHandler<UpdateProductImageCommandRequest, UpdateProductImageCommandResponse>
    {
        private readonly IProductReadRepository _productReadRepository;
        private readonly IProductImageFileReadRepository _productImageFileReadRepository;
        private readonly IProductImageFileWriteRepository _productImageFileWriteRepository;

        public UpdateProductImageCommandHandler(IProductImageFileReadRepository productImageFileReadRepository, IProductImageFileWriteRepository productImageFileWriteRepository, IProductReadRepository productReadRepository)
        {
            _productImageFileReadRepository = productImageFileReadRepository;
            _productImageFileWriteRepository = productImageFileWriteRepository;
            _productReadRepository = productReadRepository;
        }

        public async Task<UpdateProductImageCommandResponse> Handle(UpdateProductImageCommandRequest request, CancellationToken cancellationToken)
        {
            var product = await _productReadRepository.GetByIdAsync(request.ProductId);

            if (product == null)
            {
                throw new EntityNotFoundException(nameof(product), request.ProductId);
            }
            await _productReadRepository.Table.Entry(product).Collection(p => p.ProductProductImageFiles).LoadAsync(cancellationToken);

            var productImage = await product.ProductProductImageFiles.AsQueryable().FirstOrDefaultAsync(image => image.ProductImageFile.Id == Guid.Parse(request.MainImageId), cancellationToken: cancellationToken);

            //var productImage = product.ProductImageFiles.FirstOrDefault(image => image.Id == Guid.Parse(request.MainImageId));

            if (productImage == null)
            {
                throw new EntityNotFoundException(nameof(productImage), request.MainImageId);
            }

            if (productImage.IsMain) // If already main, no need to change
            {
                return new UpdateProductImageCommandResponse
                {
                    Success = true,
                    Message = "This image is already set as the main image."
                };
            }

            var mainImage = await product.ProductProductImageFiles.AsQueryable().FirstOrDefaultAsync(image => image.IsMain == true);


            if (mainImage != null)
            {
                mainImage.IsMain = false;
            }

            productImage.IsMain = true;

            var changedRecords = await _productImageFileWriteRepository.SaveAsync();

            if (changedRecords <= 0)
            {
                throw new Exception("Something happened, cannot save the changes");
            }

            return new()
            {
                Success = true,
                Message = "Main image updated successfully"
            };
        }
    }
}
