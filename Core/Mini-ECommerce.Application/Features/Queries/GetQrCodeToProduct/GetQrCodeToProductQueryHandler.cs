using MediatR;
using Mini_ECommerce.Application.Abstractions.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mini_ECommerce.Application.Features.Queries.GetQrCodeToProduct
{
    public class GetQrCodeToProductQueryHandler : IRequestHandler<GetQrCodeToProductQueryRequest, GetQrCodeToProductQueryResponse>
    {
        private readonly IProductService _productService;

        public GetQrCodeToProductQueryHandler(IProductService productService)
        {
            _productService = productService;
        }

        public async Task<GetQrCodeToProductQueryResponse> Handle(GetQrCodeToProductQueryRequest request, CancellationToken cancellationToken)
        {
            var result = await _productService.QrCodeToProductAsync(request.ProductId);

            return new GetQrCodeToProductQueryResponse()
            {
                ImageData = result
            };
        }
    }
}
