using MediatR;
using Mini_ECommerce.Application.Abstractions.Repositories;
using Mini_ECommerce.Application.Abstractions.Services;
using Mini_ECommerce.Application.Abstractions.Services.Storage;
using Mini_ECommerce.Application.Exceptions;
using Mini_ECommerce.Application.ViewModels.File;
using Mini_ECommerce.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mini_ECommerce.Application.Features.Commands.ProductImageFile.UploadProductImage
{
    public class UploadProductImageCommandHandler : IRequestHandler<UploadProductImageCommandRequest, UploadProductImageCommandResponse>
    {
        private readonly IProductService _productService;

        public UploadProductImageCommandHandler(IProductService productService)
        {
            _productService = productService;
        }

        public async Task<UploadProductImageCommandResponse> Handle(UploadProductImageCommandRequest request, CancellationToken cancellationToken)
        {
            await _productService.UploadProductImagesAsync(request.Id, request.ProductImages);

            return new UploadProductImageCommandResponse()
            {
                Success = true,
                Message = "Product images uploaded successfully!"
            };
        }
    }
}
