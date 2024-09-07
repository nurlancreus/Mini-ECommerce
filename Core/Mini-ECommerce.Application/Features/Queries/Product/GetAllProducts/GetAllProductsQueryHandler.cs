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
    public class GetAllProductsQueryHandler : IRequestHandler<GetAllProductsQueryRequest, GetAllProductsQueryResponse>
    {

       private readonly IProductService _productService;

        public GetAllProductsQueryHandler(IProductService productService)
        {
            _productService = productService;
        }

        public async Task<GetAllProductsQueryResponse> Handle(GetAllProductsQueryRequest request, CancellationToken cancellationToken)
        {
            var result = await _productService.GetAllProducts(request.Page, request.PageSize);

            return new GetAllProductsQueryResponse()
            {
                PageSize = result.PageSize,
                Page = result.CurrentPage,
                TotalItems = result.TotalItems,
                TotalPages = result.PageCount,
                Products = result.Products.Select(p =>
                {
                    return new GetProductVM()
                    {
                        Id = p.Id,
                        Name = p.Name,
                        Price = p.Price,
                        Stock = p.Stock,
                        CreatedAt = p.CreatedAt,
                        ProductImageFiles = p.ProductImageFiles.Select(i => new GetProductImageFileVM ()
                        {
                            Id = i.Id,
                            FileName = i.FileName,
                            Path = i.Path,
                            IsMain = i.IsMain,
                            CreatedAt = i.CreatedAt,
                        }).ToList()
                    };
                }).ToList()
            };
        }
    }
}
