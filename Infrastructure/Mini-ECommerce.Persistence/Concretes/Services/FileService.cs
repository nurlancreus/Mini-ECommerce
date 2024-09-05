using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Mini_ECommerce.Application.Abstractions.Repositories;
using Mini_ECommerce.Application.Abstractions.Services;
using Mini_ECommerce.Application.Abstractions.Services.Storage;
using Mini_ECommerce.Application.DTOs.File;
using Mini_ECommerce.Application.DTOs.Pagination;
using Mini_ECommerce.Application.Exceptions;
using Mini_ECommerce.Domain.Entities;
using Mini_ECommerce.Domain.Entities.Base;
using Mini_ECommerce.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace Mini_ECommerce.Persistence.Concretes.Services
{
    public class FileService : IFileService
    {
        private readonly IStorageService _storageService;
        private readonly IPaginationService _paginationService;
        private readonly ILogger<FileService> _logger;

        public FileService(IStorageService storageService, IPaginationService paginationService, ILogger<FileService> logger)
        {
            _storageService = storageService;
            _paginationService = paginationService;
            _logger = logger;
        }

        public async Task DeleteAsync<T>(string id, IWriteRepository<T> writeRepository, IReadRepository<T> readRepository) where T : AppFile
        {
            var file = await readRepository.GetByIdAsync(id);

            if (file == null)
            {
                _logger.LogError($"File with ID {id} not found.");
                throw new EntityNotFoundException(nameof(file));
            }

            // Separate database and storage operations
            bool isFileRemoved = false;
            bool isFileDeletedFromStorage = false;

            // Transaction scope for database operations
            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    // 1. Remove file from the database
                    isFileRemoved = await writeRepository.RemoveAsync(file.Id.ToString());

                    if (isFileRemoved)
                    {
                        int changedRecords = await writeRepository.SaveAsync();
                        if (changedRecords > 0)
                        {
                            _logger.LogInformation($"File with ID {id} successfully removed from the database.");

                            // Mark scope as complete to finalize DB transaction
                            scope.Complete();
                        }
                        else
                        {
                            throw new Exception($"Failed to commit database changes for file ID {id}");
                        }
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError($"Error during file deletion from the database with ID {id}: {ex.Message}");
                    throw;
                }
            }

            // Storage deletion must happen after the DB transaction completes
            if (isFileRemoved)
            {
                try
                {
                    bool isFileExistInStorage = await _storageService.HasFileAsync(file.Path, file.FileName);
                    if (isFileExistInStorage)
                    {
                        await _storageService.DeleteAsync(file.Path, file.FileName);
                        _logger.LogInformation($"File {file.FileName} deleted from storage.");
                        isFileDeletedFromStorage = true;
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError($"Error during file deletion from storage for ID {id}: {ex.Message}");
                    // Roll back the DB transaction by re-adding the file
                    _logger.LogInformation($"Rolling back DB changes for file ID {id}, since storage deletion failed.");
                    await writeRepository.AddAsync(file);
                    await writeRepository.SaveAsync();
                    throw new Exception($"Failed to delete file from storage, DB rollback completed for file ID {id}");
                }
            }
        }

        public async Task<GetAppFileDTO> GetAsync<T>(string id, IReadRepository<T> readRepository) where T : AppFile
        {
            var file = await readRepository.GetByIdAsync(id);

            if (file == null)
            {
                _logger.LogError($"File with ID {id} not found.");
                throw new EntityNotFoundException(nameof(file));
            }

            _logger.LogInformation($"File with ID {id} retrieved successfully.");
            return new GetAppFileDTO()
            {
                Id = file.Id.ToString(),
                FileName = file.FileName,
                Path = file.Path,
                CreatedAt = file.CreatedAt,
            };
        }

        public async Task<GetAppFilesDTO> GetFilesAsync<T>(int page, int size, string? pathName, IReadRepository<T> readRepository) where T : AppFile
        {
            _logger.LogInformation($"Fetching files for path: {pathName}, page: {page}, size: {size}.");

            var query = readRepository.Table.AsNoTracking();

            if (pathName != null)
            {
                query = query.Where(f => f.Path == pathName);
            }

            var paginationRequest = new PaginationRequestDTO()
            {
                Page = page,
                PageSize = size,
            };

            var (totalItems, pageSize, currentPage, totalPages, paginatedQuery) = await _paginationService.ConfigurePaginationAsync(paginationRequest, query);

            _logger.LogInformation($"Returning {paginatedQuery.Count()} files for path {pathName}.");

            return new GetAppFilesDTO()
            {
                CurrentPage = currentPage,
                PageSize = pageSize,
                PageCount = totalPages,
                TotalItems = totalItems,
                Files = paginatedQuery.Select(f => new GetAppFileDTO()
                {
                    Id = f.Id.ToString(),
                    FileName = f.FileName,
                    Path = f.Path,
                    CreatedAt = f.CreatedAt,
                }).ToList(),
            };
        }

        public async Task UploadAsync<T>(string pathName, FormFileCollection formFiles, IWriteRepository<T> writeRepository, Func<string, string, StorageType, bool> addFile) where T : AppFile
        {
            using (var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    _logger.LogInformation($"Uploading files to path: {pathName}.");
                    List<(string fileName, string pathOrContainerName)> results = await _storageService.UploadAsync(pathName, formFiles);

                    foreach (var (fileName, pathOrContainerName) in results)
                    {

                        bool isAdded = addFile(fileName, pathOrContainerName, _storageService.StorageName);

                        if (isAdded)
                        {
                            _logger.LogInformation($"File {fileName} successfully added to the database.");
                        }
                        else
                        {
                            _logger.LogWarning($"File {fileName} could not be added to the database. Deleting from storage.");
                            await _storageService.DeleteAsync(pathOrContainerName, fileName);
                        }
                    }

                    await writeRepository.SaveAsync();
                    scope.Complete();
                }
                catch (Exception ex)
                {
                    _logger.LogError($"Error during file upload to path {pathName}: {ex.Message}");
                    throw;
                }
            }
        }
    }
}
