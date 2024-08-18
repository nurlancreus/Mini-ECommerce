using MediatR;
using Mini_ECommerce.Application.Abstractions.Repositories;
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

        public CreateProductCommandHandler(IProductWriteRepository productWriteRepository)
        {
            _productWriteRepository = productWriteRepository;
        }

        public async Task<CreateProductCommandResponse> Handle(CreateProductCommandRequest request, CancellationToken cancellationToken)
        {
            bool isAdded = await _productWriteRepository.AddAsync(new() { Name = request.Name, Price = request.Price, Stock = request.Stock });

            if (!isAdded)
            {
                throw new Exception("Cannot add Product");
            }

            await _productWriteRepository.SaveAsync();


            var response = new CreateProductCommandResponse() { Message = "Product Created Successfully!", };

            return response;
        }
    }
}
