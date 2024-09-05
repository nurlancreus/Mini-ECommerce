using Microsoft.EntityFrameworkCore;
using Mini_ECommerce.Application.Abstractions.Services;
using Mini_ECommerce.Application.DTOs.Pagination;
using Mini_ECommerce.Application.Exceptions;
using Mini_ECommerce.Domain.Entities;
using Mini_ECommerce.Domain.Entities.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mini_ECommerce.Infrastructure.Concretes.Services
{
    public class PaginationService : IPaginationService
    {
        public async Task<PaginationResponseDTO<T>> ConfigurePaginationAsync<T>(PaginationRequestDTO paginationRequestDTO, IQueryable<T> entities) where T : IBase
        {
            // Validate PageSize
            if (paginationRequestDTO.PageSize <= 0)
            {
                throw new InvalidPaginationException(PaginationErrorType.InvalidPageSize, paginationRequestDTO.PageSize);
            }

            // Get total items
            var totalItems = await entities.CountAsync();
            var totalPages = (int)Math.Ceiling(totalItems / (double)paginationRequestDTO.PageSize);

            // Adjust totalPages to be at least 1
            totalPages = totalPages == 0 ? 1 : totalPages;

            // Validate PageNumber
            if (paginationRequestDTO.Page < 1 || paginationRequestDTO.Page > totalPages)
            {
                throw new InvalidPaginationException(PaginationErrorType.InvalidPageNumber, paginationRequestDTO.Page);
            }

            // Calculate pagination
            var skip = (paginationRequestDTO.Page - 1) * paginationRequestDTO.PageSize;
            var paginatedQuery = entities
                .Skip(skip)
                .Take(paginationRequestDTO.PageSize);

            // Return Pagination response
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
