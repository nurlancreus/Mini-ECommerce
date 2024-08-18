using MediatR;
using Mini_ECommerce.Application.Abstractions.Repositories;
using Mini_ECommerce.Application.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mini_ECommerce.Application.Features.Commands.Product.UpdateProduct
{
    public class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommandRequest, UpdateProductCommandResponse>
    {
        readonly IProductReadRepository _productReadRepository;
        readonly IProductWriteRepository _productWriteRepository;

        public UpdateProductCommandHandler(IProductReadRepository productReadRepository, IProductWriteRepository productWriteRepository)
        {
            _productReadRepository = productReadRepository;
            _productWriteRepository = productWriteRepository;
        }

        public async Task<UpdateProductCommandResponse> Handle(UpdateProductCommandRequest request, CancellationToken cancellationToken)
        {
            var product = await _productReadRepository.GetByIdAsync(request.Id!);

            if (product == null)
            {
                throw new EntityNotFoundException(nameof(product), request.Id);
            }

            product.Name = request.Name;
            product.Price = request.Price;
            product.Stock = request.Stock;

            await _productWriteRepository.SaveAsync();

            var response = new UpdateProductCommandResponse() { Product = new() { Id = product.Id, Name = product.Name, Price = product.Price, Stock = product.Stock, CreatedAt = product.CreatedAt, UpdatedAt = product.UpdatedAt } };

            return response;
        }
    }
}
