using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Mini_ECommerce.Application.Abstractions.Hubs;
using Mini_ECommerce.Application.Abstractions.Repositories;
using Mini_ECommerce.Application.Abstractions.Services;
using Mini_ECommerce.Application.DTOs.File;
using Mini_ECommerce.Application.DTOs.Pagination;
using Mini_ECommerce.Application.DTOs.Product;
using Mini_ECommerce.Application.Exceptions;
using Mini_ECommerce.Domain.Entities;
using Mini_ECommerce.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Mini_ECommerce.Persistence.Concretes.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductHubService _productHubService;
        private readonly IProductReadRepository _productReadRepository;
        private readonly IProductWriteRepository _productWriteRepository;
        private readonly IQRCodeService _qRCodeService;
        private readonly IPaginationService _paginationService;
        private readonly IFileService _fileService;
        private readonly IProductImageFileWriteRepository _productImageFileWriteRepository;
        private readonly IProductImageFileReadRepository _productImageFileReadRepository;

        private const string ProductImageDirectory = "product-images";

        public ProductService(IProductReadRepository productReadRepository, IProductWriteRepository productWriteRepository, IQRCodeService qRCodeService, IProductHubService productHubService, IPaginationService paginationService, IFileService fileService, IProductImageFileWriteRepository productImageFileWriteRepository, IProductImageFileReadRepository productImageFileReadRepository)
        {
            _productReadRepository = productReadRepository;
            _productWriteRepository = productWriteRepository;
            _qRCodeService = qRCodeService;
            _productHubService = productHubService;
            _paginationService = paginationService;
            _fileService = fileService;
            _productImageFileWriteRepository = productImageFileWriteRepository;
            _productImageFileReadRepository = productImageFileReadRepository;
        }

        private async Task<string?> AddImageAsync(string fileName, string pathName, StorageType storageName)
        {
            var productImage = new ProductImageFile()
            {
                FileName = fileName,
                Path = pathName,
                Storage = storageName,
            };

            bool isAdded = await _productImageFileWriteRepository.AddAsync(productImage);

            if (isAdded)
            {
                return productImage.Id.ToString();
            }

            return null;
        }

        public async Task CreateProductAsync(CreateProductDTO createProductDTO)
        {
            var product = new Product()
            {
                Name = createProductDTO.Name,
                Price = createProductDTO.Price,
                Stock = createProductDTO.Stock
            };

            if (createProductDTO.ProductImages.Count > 0)
            {
                int counter = 0;

                var productImageIds = await _fileService.UploadAsync(ProductImageDirectory, createProductDTO.ProductImages, _productImageFileWriteRepository, AddImageAsync);

                foreach (var id in productImageIds)
                {
                    if (Guid.TryParse(id, out var imageId))
                    {
                        counter++;
                        product.ProductProductImageFiles.Add(new ProductProductImageFile()
                        {
                            ProductImageFileId = imageId,
                            IsMain = counter == 1,
                        });

                    }
                }
            }

            bool isAdded = await _productWriteRepository.AddAsync(product);

            if (!isAdded)
            {
                throw new Exception("Cannot add Product");
            }

            await _productWriteRepository.SaveAsync();
            await _productHubService.ProductAddedMessageAsync($"Product {createProductDTO.Name} added.");
        }
        public async Task DeleteProductAsync(string id)
        {

            bool isDeleted = await _productWriteRepository.RemoveAsync(id);

            if (!isDeleted)
            {
                throw new Exception("Could not delete product");
            }
        }

        public async Task UpdateProductAsync(UpdateProductDTO updateProductDTO)
        {
            var product = await _productReadRepository.GetByIdAsync(updateProductDTO.Id);

            if (product == null)
            {
                throw new EntityNotFoundException(nameof(product), updateProductDTO.Id);
            }

            product.Name = updateProductDTO.Name;
            product.Price = updateProductDTO.Price;
            product.Stock = updateProductDTO.Stock;

            await _productWriteRepository.SaveAsync();
        }

        public async Task<byte[]> QrCodeToProductAsync(string productId)
        {
            Product product = await _productReadRepository.GetByIdAsync(productId) ?? throw new Exception("Product not found");

            var plainObject = new
            {
                product.Id,
                product.Name,
                product.Price,
                product.Stock,
                product.CreatedAt
            };

            string plainText = JsonSerializer.Serialize(plainObject);

            return await _qRCodeService.GenerateQRCodeAsync(plainText);
        }

        public async Task StockUpdateToProductAsync(string productId, int stock)
        {
            Product product = await _productReadRepository.GetByIdAsync(productId) ?? throw new Exception("Product not found");

            product.Stock = stock;
            await _productWriteRepository.SaveAsync();
        }

        public async Task<GetAllProductsDTO> GetAllProducts(int page, int size)
        {

            var query = _productReadRepository.GetAll(false);

            // Configure pagination
            var paginationRequest = new PaginationRequestDTO
            {
                Page = page,
                PageSize = size,
            };

            var (totalItems, pageSize, currentPage, totalPages, paginatedQuery) = await _paginationService.ConfigurePaginationAsync(paginationRequest, query);

            // Fetch products with their related images (eager loading)
            var products = await paginatedQuery
                .Include(p => p.ProductProductImageFiles)
                    .ThenInclude(ppif => ppif.ProductImageFile)
                .ToListAsync();

            // Map products to DTO
            var productDTOs = products.Select(p => new GetProductDTO
            {
                Id = p.Id.ToString(),
                Name = p.Name,
                Price = p.Price,
                Stock = p.Stock,
                CreatedAt = p.CreatedAt,
                ProductImageFiles = p.ProductProductImageFiles.Select(pi => new GetProductImageFileDTO
                {
                    Id = pi.ProductImageFile.Id.ToString(),
                    FileName = pi.ProductImageFile.FileName,
                    Path = pi.ProductImageFile.Path,
                    CreatedAt = pi.ProductImageFile.CreatedAt,
                    IsMain = pi.IsMain,
                }).ToList(),
            }).ToList();

            return new GetAllProductsDTO
            {
                CurrentPage = currentPage,
                PageCount = totalPages,
                PageSize = pageSize,
                TotalItems = totalItems,
                Products = productDTOs,
            };
        }


        public async Task<GetProductDTO> GetProductByIdAsync(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                throw new ArgumentException("Product ID cannot be null or empty.");
            }

            var product = await _productReadRepository.Table
                .Include(p => p.ProductProductImageFiles)
                    .ThenInclude(ppif => ppif.ProductImageFile)
                .FirstOrDefaultAsync(p => p.Id.ToString() == id);

            if (product == null)
            {
                throw new EntityNotFoundException(nameof(product), id);
            }

            return new GetProductDTO
            {
                Id = product.Id.ToString(),
                Name = product.Name,
                Price = product.Price,
                Stock = product.Stock,
                CreatedAt = product.CreatedAt,
                ProductImageFiles = product.ProductProductImageFiles.Select(pi => new GetProductImageFileDTO
                {
                    Id = pi.ProductImageFile.Id.ToString(),
                    FileName = pi.ProductImageFile.FileName,
                    Path = pi.ProductImageFile.Path,
                    CreatedAt = pi.ProductImageFile.CreatedAt,
                    IsMain = pi.IsMain
                }).ToList(),
            };
        }

        public async Task ChangeMainImageAsync(string productImageId)
        {
            var product = await _productReadRepository.Table
                .Include(p => p.ProductProductImageFiles)
                    .ThenInclude(ppif => ppif.ProductImageFile)
                .FirstOrDefaultAsync(p => p.ProductProductImageFiles.Any(ppif => ppif.ProductImageFile.Id.ToString() == productImageId));

            if (product == null)
            {
                throw new EntityNotFoundException(nameof(product), productImageId);
            }

            var currentMainImage = product.ProductProductImageFiles.FirstOrDefault(ppif => ppif.IsMain);

            if (currentMainImage?.ProductImageFile.Id.ToString() == productImageId)
            {
                return;
            }

            if (currentMainImage != null)
            {
                currentMainImage.IsMain = false;
            }

            var newMainImage = product.ProductProductImageFiles
                .FirstOrDefault(ppif => ppif.ProductImageFile.Id.ToString() == productImageId);

            if (newMainImage == null)
            {
                throw new EntityNotFoundException("ProductImageFile", productImageId);
            }

            newMainImage.IsMain = true;

            await _productWriteRepository.SaveAsync();
        }

        public async Task UploadProductImagesAsync(string productId, FormFileCollection productImages)
        {
            var product = await _productReadRepository.Table.Include(p => p.ProductProductImageFiles).ThenInclude(ppfi => ppfi.ProductImageFile).FirstOrDefaultAsync(p => p.Id.ToString() == productId);

            if (product == null)
            {
                throw new EntityNotFoundException(nameof(product), productId);
            }

            bool isMainImageExist = product.ProductProductImageFiles.Any(ppfi => ppfi.IsMain);

            if (productImages.Count > 0)
            {
                var productImageIds = await _fileService.UploadAsync(ProductImageDirectory, productImages, _productImageFileWriteRepository, AddImageAsync);

                int counter = 0;

                foreach (var imageId in productImageIds)
                {
                    if (Guid.TryParse(imageId, out Guid parsedId))
                    {
                        counter++;
                        product.ProductProductImageFiles.Add(new ProductProductImageFile()
                        {
                            IsMain = counter == 1 && !isMainImageExist,
                            ProductImageFileId = parsedId
                        });
                    }
                }

                await _productWriteRepository.SaveAsync();
            }
        }

        public async Task DeleteProductImageAsync(string productImageId)
        {
            var product = await _productReadRepository.Table
                .Include(p => p.ProductProductImageFiles)
                    .ThenInclude(ppif => ppif.ProductImageFile)
                .FirstOrDefaultAsync(p => p.ProductProductImageFiles.Any(ppif => ppif.ProductImageFile.Id.ToString() == productImageId));

            if (product == null)
            {
                throw new EntityNotFoundException(nameof(product), productImageId);
            }

            var currentMainImage = product.ProductProductImageFiles.FirstOrDefault(ppif => ppif.IsMain);

            if (currentMainImage?.ProductImageFileId.ToString() == productImageId) 
            {
                var otherImages = product.ProductProductImageFiles.Where(ppif => !ppif.IsMain);

                if (otherImages.Any())
                {
                    otherImages.ElementAt(0).IsMain = true;
                } 
            }

            await _fileService.DeleteAsync(productImageId, _productImageFileWriteRepository, _productImageFileReadRepository);
        }

        public async Task<GetProductImagesFilesDTO> GetProductImages(string productId, int page, int size)
        {

            var product = await _productReadRepository.Table
                .Include(p => p.ProductProductImageFiles)
                    .ThenInclude(ppfi => ppfi.ProductImageFile)
                .FirstOrDefaultAsync(p => p.Id.ToString() == productId);

            if (product == null)
            {
                throw new EntityNotFoundException(nameof(product), productId);
            }

            var query = _productImageFileReadRepository.GetWhere(pi => pi.ProductProductImageFiles.Any(ppfi => ppfi.ProductId.ToString() == productId));

            var paginationRequest = new PaginationRequestDTO
            {
                Page = page,
                PageSize = size,
            };

            var (totalItems, pageSize, currentPage, totalPages, paginatedQuery) = await _paginationService.ConfigurePaginationAsync(paginationRequest, query);

            var imageMap = product.ProductProductImageFiles.ToDictionary(ppfi => ppfi.ProductImageFileId, ppfi => ppfi.IsMain);

            return new GetProductImagesFilesDTO
            {
                CurrentPage = currentPage,
                PageCount = totalPages,
                PageSize = pageSize,
                TotalItems = totalItems,
                ProductImages = paginatedQuery.Select(f => new GetProductImageFileDTO
                {
                    Id = f.Id.ToString(),
                    FileName = f.FileName,
                    Path = f.Path,
                    CreatedAt = f.CreatedAt,
                    IsMain = imageMap.ContainsKey(f.Id) && imageMap[f.Id]  
                }).ToList()
            };
        }

    }
}

