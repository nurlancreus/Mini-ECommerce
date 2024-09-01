using Mini_ECommerce.Domain.Entities.Base;
using System.Linq;

namespace Mini_ECommerce.Application.DTOs.Pagination
{
    public class PaginationResponseDTO<T> where T : IBase
    {
        public int TotalItems { get; set; } // Total number of items
        public int TotalPages { get; set; } // Total number of pages
        public int Page { get; set; } // Current page number
        public int PageSize { get; set; } // Number of items per page
        public IQueryable<T>? PaginatedQuery { get; set; } // The actual items for the current page

        // Deconstruct method for easy unpacking
        public void Deconstruct(out int totalItems, out int pageSize, out int page, out int totalPages, out IQueryable<T> paginatedQuery)
        {
            totalItems = TotalItems;
            pageSize = PageSize;
            page = Page;
            totalPages = TotalPages;
            paginatedQuery = PaginatedQuery;
        }
    }
}
