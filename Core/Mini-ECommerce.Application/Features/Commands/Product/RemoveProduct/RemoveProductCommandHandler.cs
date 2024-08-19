using MediatR;
using Mini_ECommerce.Application.Abstractions.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mini_ECommerce.Application.Features.Commands.Product.RemoveProduct
{
    public class RemoveProductCommandHandler : IRequestHandler<RemoveProductCommandRequest, RemoveProductCommandResponse>
    {
        private readonly IProductWriteRepository _productWriteRepository;

        public RemoveProductCommandHandler(IProductWriteRepository productWriteRepository)
        {
            _productWriteRepository = productWriteRepository;
        }

        public async Task<RemoveProductCommandResponse> Handle(RemoveProductCommandRequest request, CancellationToken cancellationToken)
        {
            bool isDeleted = await _productWriteRepository.RemoveAsync(request.Id);

            if (!isDeleted)
            {
                throw new Exception("Could not delete product");
            }

            return new RemoveProductCommandResponse();
        }
    }
}
