using Microsoft.AspNetCore.Http;
using Mini_ECommerce.Application.Abstractions.Repositories;
using Mini_ECommerce.Application.DTOs.File;
using Mini_ECommerce.Domain.Entities;
using Mini_ECommerce.Domain.Entities.Base;
using Mini_ECommerce.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mini_ECommerce.Application.Abstractions.Services
{
    public interface IFileService
    {
        public Task UploadAsync<T>(string pathName, FormFileCollection formFiles, IWriteRepository<T> fileWriteRepository, Func<string, string, StorageType, bool> addFile) where T : AppFile;
        public Task DeleteAsync<T>(string id, IWriteRepository<T> fileWriteRepository, IReadRepository<T> readRepository) where T : AppFile;
        public Task<GetAppFilesDTO> GetFilesAsync<T>(int page, int size, string? pathName, IReadRepository<T> fileReadRepository) where T : AppFile;
        public Task<GetAppFileDTO> GetAsync<T>(string id, IReadRepository<T> fileReadRepository) where T : AppFile;
    }
}
