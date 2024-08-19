using MediatR;
using Mini_ECommerce.Application.Abstractions.Repositories;
using Mini_ECommerce.Application.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mini_ECommerce.Application.Features.Queries.ProductImageFile.GetProductImages
{
    public class GetProductImagesQueryHandler : IRequestHandler<GetProductImagesQueryRequest, GetProductImagesQueryResponse>
    {
        private readonly IProductReadRepository _productReadRepository;

        public GetProductImagesQueryHandler(IProductReadRepository productReadRepository)
        {
            _productReadRepository = productReadRepository;
        }

        public async Task<GetProductImagesQueryResponse> Handle(GetProductImagesQueryRequest request, CancellationToken cancellationToken)
        {
            var product = await _productReadRepository.GetByIdAsync(request.Id);

            if (product == null)
            {
                throw new EntityNotFoundException(nameof(product), request.Id);
            }

            await _productReadRepository.Table.Entry(product).Collection(p => p.ProductImageFiles).LoadAsync(cancellationToken);

            var response = new GetProductImagesQueryResponse()
            {
                Images = product.ProductImageFiles.Select(i =>
             new ViewModels.ProductImageFile.GetProductImageFileVM() { Id = i.Id, FileName = i.FileName, Path = i.Path, CreatedAt = i.CreatedAt }).ToList()
            };

            return response;
        }
    }
}
