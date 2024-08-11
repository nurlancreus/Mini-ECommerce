using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Mini_ECommerce.Application.Abstractions.Repositories.Product;
using Mini_ECommerce.Application.RequestParameters;

namespace Mini_ECommerce.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]


    public class ProductsController : ControllerBase
    {
        private readonly IProductReadRepository _productReadRepository;

        public ProductsController(IProductReadRepository productReadRepository)
        {
            _productReadRepository = productReadRepository;
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
    }
}
