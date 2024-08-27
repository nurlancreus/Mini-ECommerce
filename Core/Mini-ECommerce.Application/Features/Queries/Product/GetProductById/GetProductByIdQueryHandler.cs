using MediatR;
using Mini_ECommerce.Application.Abstractions.Repositories;
using Mini_ECommerce.Application.Exceptions;
using Mini_ECommerce.Application.ViewModels.Address;
using Mini_ECommerce.Application.ViewModels.Customer;
using Mini_ECommerce.Application.ViewModels.Order;
using Mini_ECommerce.Application.ViewModels.Product;
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
        private readonly IOrderReadRepository _orderReadRepository;

        public GetProductByIdQueryHandler(IProductReadRepository productReadRepository, IOrderReadRepository orderReadRepository)
        {
            _productReadRepository = productReadRepository;
            _orderReadRepository = orderReadRepository;
        }

        public async Task<GetProductByIdQueryResponse> Handle(GetProductByIdQueryRequest request, CancellationToken cancellationToken)
        {
            var product = await _productReadRepository.GetByIdAsync(request.Id, false);

            if (product == null)
            {
                throw new EntityNotFoundException(nameof(product), request.Id);
            }

            // Explicitly load related entities

            /*
            await _productReadRepository.Table.Entry(product).Collection(p => p.Orders).LoadAsync(cancellationToken);
            foreach (var order in product.Orders)
            {
                await _orderReadRepository.Table.Entry(order).Reference(o => o.Customer).LoadAsync(cancellationToken);
            }
            */

            // Map the product and its related entities to the response view model
            var response = new GetProductByIdQueryResponse()
            {
                Product = new GetProductVM
                {
                    Id = product.Id.ToString(),
                    Name = product.Name,
                    Price = product.Price,
                    Stock = product.Stock,
                    CreatedAt = product.CreatedAt,
                    UpdatedAt = product.UpdatedAt,
                }
            };

            return response;
        }
    }
}
