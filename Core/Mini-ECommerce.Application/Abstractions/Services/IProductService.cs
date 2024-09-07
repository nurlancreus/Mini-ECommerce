using Microsoft.AspNetCore.Http;
using Mini_ECommerce.Application.DTOs.File;
using Mini_ECommerce.Application.DTOs.Product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mini_ECommerce.Application.Abstractions.Services
{
    public interface IProductService
    {
        Task<GetAllProductsDTO> GetAllProducts(int page, int size);
        Task<GetProductDTO> GetProductByIdAsync (string id);
        Task CreateProductAsync(CreateProductDTO createProductDTO);
        Task UpdateProductAsync(UpdateProductDTO updateProductDTO);
        Task UploadProductImagesAsync(string productId, FormFileCollection productImages);
        Task ChangeMainImageAsync(string productId, string productImageId);
        Task DeleteProductImageAsync(string productId, string productImageId);
        Task DeleteProductAsync(string id);
        Task<GetProductImagesFilesDTO> GetProductImages(string productId, int page, int size);
        Task<byte[]> QrCodeToProductAsync(string productId);
        Task StockUpdateToProductAsync(string productId, int stock);
    }
}
