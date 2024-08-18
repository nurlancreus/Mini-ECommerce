using MediatR;
using Mini_ECommerce.Application.Abstractions.Repositories;
using Mini_ECommerce.Application.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mini_ECommerce.Application.Features.Queries.Product.GetProductById
{
    public class GetProductByIdQueryHandler : IRequestHandler<GetProductByIdQueryRequest, GetProductByIdQueryResponse>
    {
        private readonly IProductReadRepository _productReadRepository;

        public GetProductByIdQueryHandler(IProductReadRepository productReadRepository)
        {
            _productReadRepository = productReadRepository;
        }

        public async Task<GetProductByIdQueryResponse> Handle(GetProductByIdQueryRequest request, CancellationToken cancellationToken)
        {
            var product = await _productReadRepository.GetByIdAsync(request.Id, false);

            if(product == null)
            {
                throw new EntityNotFoundException(nameof(product), request.Id);
            }

            var response = new GetProductByIdQueryResponse() { Product = new() { Id = product.Id, Name = product.Name, Price = product.Price, Stock = product.Stock, CreatedAt = product.CreatedAt, UpdatedAt = product.UpdatedAt } };

            return response;
        }
    }
}
