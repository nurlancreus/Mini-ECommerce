using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mini_ECommerce.Application.DTOs.Pagination
{
    public class PaginationRequestDTO
    {
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 5;
    }
}
