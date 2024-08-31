using Microsoft.EntityFrameworkCore;
using Mini_ECommerce.Application.Abstractions.Services;
using Mini_ECommerce.Application.DTOs.Pagination;
using Mini_ECommerce.Application.Exceptions;
using Mini_ECommerce.Domain.Entities;
using Mini_ECommerce.Domain.Entities.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mini_ECommerce.Infrastructure.Concretes.Services
{
    public class PaginationService : IPaginationService
    {
        public async Task<PaginationResponseDTO<T>> ConfigurePaginationAsync<T>(PaginationRequestDTO paginationRequestDTO, IQueryable<T> entities) where T : IBase
        {
            var totalItems = await entities.CountAsync();

            if (paginationRequestDTO.PageSize <= 0)
            {
                throw new InvalidPaginationException(PaginationErrorType.InvalidPageSize, paginationRequestDTO.PageSize);
            }

            var totalPages = (int)Math.Ceiling(totalItems / (double)paginationRequestDTO.PageSize);

            totalPages = totalPages == 0 ? 1 : totalPages;

            if (paginationRequestDTO.Page < 1 || paginationRequestDTO.Page > totalPages)
            {
                throw new InvalidPaginationException(PaginationErrorType.InvalidPageNumber, paginationRequestDTO.Page);
            }

            var paginatedQuery = entities
                .Skip((paginationRequestDTO.Page - 1) * paginationRequestDTO.PageSize)
                .Take(paginationRequestDTO.PageSize);
                //.Take(((paginationRequestDTO.Page - 1) * paginationRequestDTO.PageSize)..paginationRequestDTO.PageSize);

            var response = new PaginationResponseDTO<T>()
            {
                Page = paginationRequestDTO.Page,
                PageSize = paginationRequestDTO.PageSize,
                TotalItems = totalItems,
                TotalPages = totalPages,
                PaginatedQuery = paginatedQuery
            };

            return response;
        }

    }
}
