using MediatR;
using Mini_ECommerce.Application.Abstractions.Hubs;
using Mini_ECommerce.Application.Abstractions.Repositories;
using Mini_ECommerce.Application.Abstractions.Services;
using Mini_ECommerce.Application.DTOs.Product;
using Mini_ECommerce.Application.Exceptions;
using Mini_ECommerce.Domain.Entities;
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

        public ProductService(IProductReadRepository productReadRepository, IProductWriteRepository productWriteRepository, IQRCodeService qRCodeService, IProductHubService productHubService)
        {
            _productReadRepository = productReadRepository;
            _productWriteRepository = productWriteRepository;
            _qRCodeService = qRCodeService;
            _productHubService = productHubService;
        }

        public async Task CreateProductAsync(CreateProductDTO createProductDTO)
        {

            bool isAdded = await _productWriteRepository.AddAsync(new() { Name = createProductDTO.Name, Price = createProductDTO.Price, Stock = createProductDTO.Stock });

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

    }
}
