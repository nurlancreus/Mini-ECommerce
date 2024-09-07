using MediatR;
using Mini_ECommerce.Application.Abstractions.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mini_ECommerce.Application.Features.Commands.ProductImageFile.RemoveProductImage
{
    public class RemoveProductImageCommandHandler : IRequestHandler<RemoveProductImageCommandRequest, RemoveProductImageCommandResponse>
    {
        private readonly IProductService _productService;

        public RemoveProductImageCommandHandler(IProductService productService)
        {
            _productService = productService;
        }

        public async Task<RemoveProductImageCommandResponse> Handle(RemoveProductImageCommandRequest request, CancellationToken cancellationToken)
        {
            await _productService.DeleteProductImageAsync(request.ProductId, request.ProductImageId);

            return new RemoveProductImageCommandResponse()
            {
                Success = true,
                Message = "Product image deleted successfully!"
            };
        }
    }
}
