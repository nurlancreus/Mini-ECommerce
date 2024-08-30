using Mini_ECommerce.Application.DTOs.Pagination;
using Mini_ECommerce.Domain.Entities.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mini_ECommerce.Application.Abstractions.Services
{
    public interface IPaginationService
    {
        Task<PaginationResponseDTO<T>> ConfigurePaginationAsync<T>(PaginationRequestDTO paginationRequestDTO, IQueryable<T> entities) where T : IBase;
    }
}
