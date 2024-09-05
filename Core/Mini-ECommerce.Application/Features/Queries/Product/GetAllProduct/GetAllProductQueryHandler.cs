using MediatR;
using Microsoft.EntityFrameworkCore;
using Mini_ECommerce.Application.Abstractions.Repositories;
using Mini_ECommerce.Application.Abstractions.Services;
using Mini_ECommerce.Application.DTOs.Pagination;
using Mini_ECommerce.Application.Exceptions;
using Mini_ECommerce.Application.ViewModels.Address;
using Mini_ECommerce.Application.ViewModels.Customer;
using Mini_ECommerce.Application.ViewModels.File;
using Mini_ECommerce.Application.ViewModels.Order;
using Mini_ECommerce.Application.ViewModels.Product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mini_ECommerce.Application.Features.Queries.Product.GetAllProduct
{
    public class GetAllProductQueryHandler : IRequestHandler<GetAllProductQueryRequest, GetAllProductQueryResponse>
    {

        private readonly IProductReadRepository _productReadRepository;
        private readonly IPaginationService _paginationService;

        public GetAllProductQueryHandler(IProductReadRepository productReadRepository, IPaginationService paginationService)
        {
            _productReadRepository = productReadRepository;
            _paginationService = paginationService;
        }

        public async Task<GetAllProductQueryResponse> Handle(GetAllProductQueryRequest request, CancellationToken cancellationToken)
        {
            // Fetch the queryable data source
            var query = _productReadRepository.GetAll(false);

            var paginationRequest = new PaginationRequestDTO()
            {
                Page = request.Page,
                PageSize = request.PageSize,
            };

            var (totalItems, pageSize, currentPage, totalPages, paginatedQuery) = await _paginationService.ConfigurePaginationAsync(paginationRequest, query);

            // Fetch the requested page of products
            var products = await paginatedQuery
                .Include(p => p.ProductProductImageFiles)
                .ThenInclude(p => p.ProductImageFile)
                .Select(p => new GetProductVM
                {
                    Id = p.Id.ToString(),
                    Name = p.Name,
                    Price = p.Price,
                    Stock = p.Stock,
                    CreatedAt = p.CreatedAt,
                    ProductImageFiles = p.ProductProductImageFiles.Select(image => new GetProductImageFileVM
                    {
                        IsMain = image.IsMain,
                        FileName = image.ProductImageFile.FileName,
                       // Id = image.ProductImageFile.Id,
                        Path = image.ProductImageFile.Path,
                        CreatedAt = image.ProductImageFile.CreatedAt
                    }).ToList()
                })
                .ToListAsync(cancellationToken: cancellationToken);

            // Create a response with pagination metadata
            var response = new GetAllProductQueryResponse()
            {
                TotalItems = totalItems,
                Page = request.Page,
                PageSize = request.PageSize,
                TotalPages = totalPages,
                Products = products
            };

            return response;
        }
    }
}
