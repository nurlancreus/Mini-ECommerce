using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Mini_ECommerce.Application.Abstractions.Repositories;
using Mini_ECommerce.Application.Abstractions.Services;
using Mini_ECommerce.Application.Abstractions.Services.Storage;
using Mini_ECommerce.Application.RequestParameters;
using Mini_ECommerce.Domain.Entities;
using Mini_ECommerce.Domain.Enums;

namespace Mini_ECommerce.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]


    public class ProductsController : ControllerBase
    {
        private readonly IProductReadRepository _productReadRepository;
        private readonly IProductWriteRepository _productWriteRepository;
        private readonly IWebHostEnvironment _webHostEnvironment;
        // private readonly IFileService _fileService;
        private readonly IStorageService _storageService;

        private readonly IProductImageFileWriteRepository _productImageFileWriteRepository;
        private readonly IProductImageFileReadRepository _productImageFileReadRepository;

        private readonly IInvoiceFileWriteRepository _invoiceFileWriteRepository;
        private readonly IInvoiceFileReadRepository _invoiceFileReadRepository;

        public ProductsController(IProductReadRepository productReadRepository, IProductWriteRepository productWriteRepository, IWebHostEnvironment webHostEnvironment, IStorageService storageService, IProductImageFileWriteRepository productImageFileWriteRepository, IProductImageFileReadRepository productImageFileReadRepository, IInvoiceFileWriteRepository invoiceFileWriteRepository, IInvoiceFileReadRepository invoiceFileReadRepository)
        {
            _productReadRepository = productReadRepository;
            _productWriteRepository = productWriteRepository;
            _webHostEnvironment = webHostEnvironment;
            // _fileService = fileService;
            _storageService = storageService;
            _productImageFileWriteRepository = productImageFileWriteRepository;
            _productImageFileReadRepository = productImageFileReadRepository;
            _invoiceFileWriteRepository = invoiceFileWriteRepository;
            _invoiceFileReadRepository = invoiceFileReadRepository;
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] Pagination paginationOptions)
        {
            // Fetch the queryable data source
            var query = _productReadRepository.GetAll(false);

            // Count the total items asynchronously
            var totalItems = await query.CountAsync();

            // Calculate total number of pages
            var totalPages = (int)Math.Ceiling(totalItems / (double)paginationOptions.PageSize);

            // Validate PageSize (Ensure it is positive and within a reasonable limit)
            if (paginationOptions.PageSize <= 0 || paginationOptions.PageSize > 100)
            {
                return BadRequest("Page size must be a positive number and cannot exceed 100.");
            }

            // Validate Page (Ensure it is within the valid range)
            if (paginationOptions.Page < 1 || paginationOptions.Page > totalPages)
            {
                return BadRequest($"Page number must be between 1 and {totalPages}.");
            }

            // Fetch the requested page of products
            var products = await query
                .Skip((paginationOptions.Page - 1) * paginationOptions.PageSize)
                .Take(paginationOptions.PageSize)
                .Select(p => new { p.Id, p.Name, p.Price, p.Stock, p.CreatedAt })
                .ToListAsync();

            // Create a response with pagination metadata
            var response = new
            {
                TotalItems = totalItems,
                paginationOptions.Page,
                paginationOptions.PageSize,
                TotalPages = totalPages,
                Items = products
            };

            return Ok(response);
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> GetFiles()
        {
            var files = await _storageService.GetFilesAsync("files");

            return Ok(files);
        }

        [HttpDelete("[action]/{id}")]
        public async Task<IActionResult> DeleteFile([FromRoute] string id)
        {
            var file = await _productImageFileReadRepository.GetByIdAsync(id);

            if (file != null)
            {
                bool isDeleted = await _productImageFileWriteRepository.RemoveAsync(id);

                if (isDeleted)
                {
                    await _productImageFileWriteRepository.SaveAsync();
                    await _storageService.DeleteAsync("files", file.FileName);
                }
            }

            return Ok();
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> Upload([FromForm] IFormFileCollection files, [FromQuery] string? productId)
        {
            if (files == null || files.Count == 0)
            {
                return BadRequest("No files were provided for upload.");
            }

            try
            {
                // var uploadResults = await _fileService.UploadAsync("resource/product-images", files);
                var uploadResults = await _storageService.UploadAsync("files", files);

                await _productImageFileWriteRepository.AddRangeAsync(uploadResults.Select(result =>
                     new ProductImageFile { FileName = result.fileName, Path = result.pathOrContainerName, Storage = Enum.Parse<StorageType>(_storageService.StorageName) }
                ).ToList());

                await _productImageFileWriteRepository.SaveAsync();

                if (uploadResults == null)
                {
                    return StatusCode(500, "An error occurred during the upload process.");
                }

                return Ok(new
                {
                    Message = "Files uploaded successfully.",
                    Files = uploadResults
                });
            }
            catch (Exception ex)
            {
                // Optionally log the exception
                //_logger.LogError(ex, "File upload failed.");

                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }

    }
}
