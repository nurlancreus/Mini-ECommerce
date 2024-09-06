using MediatR;
using Mini_ECommerce.Application.Abstractions.Hubs;
using Mini_ECommerce.Application.Abstractions.Repositories;
using Mini_ECommerce.Application.Abstractions.Services;
using Mini_ECommerce.Application.DTOs.Product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Mini_ECommerce.Application.Features.Commands.Product.CreateProduct
{
    public class CreateProductCommandHandler : IRequestHandler<CreateProductCommandRequest, CreateProductCommandResponse>
    {
        private readonly IProductWriteRepository _productWriteRepository;
        private readonly IProductService _productService;

        public CreateProductCommandHandler(IProductWriteRepository productWriteRepository, IProductService productService)
        {
            _productService = productService;
        }

        public async Task<CreateProductCommandResponse> Handle(CreateProductCommandRequest request, CancellationToken cancellationToken)
        {

            await _productService.CreateProductAsync(new CreateProductDTO()
            {
                Name = request.Name,
                Price = request.Price,
                Stock = request.Stock,
                ProductImages = request.ProductImages,
            });

            return new CreateProductCommandResponse() { Success = true, Message = "Product Created Successfully!", };
        }
    }
}
