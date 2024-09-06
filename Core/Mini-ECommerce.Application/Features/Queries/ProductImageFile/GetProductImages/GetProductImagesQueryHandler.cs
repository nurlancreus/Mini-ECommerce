using MediatR;
using Mini_ECommerce.Application.Abstractions.Repositories;
using Mini_ECommerce.Application.Abstractions.Services;
using Mini_ECommerce.Application.Exceptions;
using Mini_ECommerce.Application.ViewModels.File;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mini_ECommerce.Application.Features.Queries.ProductImageFile.GetProductImages
{
    public class GetProductImagesQueryHandler : IRequestHandler<GetProductImagesQueryRequest, GetProductImagesQueryResponse>
    {
        private readonly IProductService _productService;

        public GetProductImagesQueryHandler(IProductService productService)
        {
            _productService = productService;
        }

        public async Task<GetProductImagesQueryResponse> Handle(GetProductImagesQueryRequest request, CancellationToken cancellationToken)
        {
            var result = await _productService.GetProductImages(request.Id, request.Page, request.PageSize);

            return new GetProductImagesQueryResponse()
            {
                Page = result.CurrentPage,
                PageSize = result.PageSize,
                TotalItems = result.TotalItems,
                TotalPages = result.PageCount,
                Images = result.ProductImages.Select(pi => new GetProductImageFileVM ()
                {
                    Id = pi.Id,
                    FileName = pi.FileName,
                    Path = pi.Path,
                    IsMain = pi.IsMain,
                    CreatedAt = pi.CreatedAt,
                }).ToList()
            };
        }
    }
}
