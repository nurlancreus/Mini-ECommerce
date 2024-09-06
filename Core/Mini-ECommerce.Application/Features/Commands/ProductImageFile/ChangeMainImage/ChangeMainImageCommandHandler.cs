using MediatR;
using Mini_ECommerce.Application.Abstractions.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mini_ECommerce.Application.Features.Commands.ProductImageFile.ChangeMainImage
{
    public class ChangeMainImageCommandHandler : IRequestHandler<ChangeMainImageCommandRequest, ChangeMainImageCommandResponse>
    {
        private readonly IProductService _productService;

        public ChangeMainImageCommandHandler(IProductService productService)
        {
            _productService = productService;
        }

        public async Task<ChangeMainImageCommandResponse> Handle(ChangeMainImageCommandRequest request, CancellationToken cancellationToken)
        {
            await _productService.ChangeMainImageAsync(request.ProductImageId);

            return new ChangeMainImageCommandResponse()
            {
                Success = true,
                Message = "Main image changed successfully!"
            };
        }
    }
}
