using MediatR;
using Microsoft.EntityFrameworkCore;
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

namespace Mini_ECommerce.Application.Features.Queries.Product.GetAllProduct
{
    public class GetAllProductQueryHandler : IRequestHandler<GetAllProductQueryRequest, GetAllProductQueryResponse>
    {

        private readonly IProductReadRepository _productReadRepository;

        public GetAllProductQueryHandler(IProductReadRepository productReadRepository)
        {
            _productReadRepository = productReadRepository;
        }

        public async Task<GetAllProductQueryResponse> Handle(GetAllProductQueryRequest request, CancellationToken cancellationToken)
        {
            // Fetch the queryable data source
            var query = _productReadRepository.GetAll(false);

            // Count the total items asynchronously
            var totalItems = await query.CountAsync(cancellationToken: cancellationToken);

            // Calculate total number of pages
            var totalPages = (int)Math.Ceiling(totalItems / (double)request.PageSize);

            if (request.PageSize <= 0 || request.PageSize > 100)
            {
                throw new InvalidPaginationException(PaginationErrorType.InvalidPageSize, request.PageSize);
            }

            if (request.Page < 1 || request.Page > totalPages)
            {
                throw new InvalidPaginationException(PaginationErrorType.InvalidPageNumber, request.Page);
            }

            // Fetch the requested page of products
            var products = await query
                .Skip((request.Page - 1) * request.PageSize)
                .Take(request.PageSize)
                .Include(p => p.ProductProductImageFiles)
                .ThenInclude(p => p.ProductImageFile)
                .Select(p => new GetProductVM
                {
                    Id = p.Id,
                    Name = p.Name,
                    Price = p.Price,
                    Stock = p.Stock,
                    CreatedAt = p.CreatedAt,
                    ProductImageFiles = p.ProductProductImageFiles.Select(image => new ViewModels.ProductImageFile.GetProductImageFileVM
                    {
                        IsMain = image.IsMain,
                        FileName = image.ProductImageFile.FileName,
                        Id = image.ProductImageFile.Id,
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
