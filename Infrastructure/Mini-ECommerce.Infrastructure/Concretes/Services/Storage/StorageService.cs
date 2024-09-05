using Microsoft.AspNetCore.Http;
using Mini_ECommerce.Application.Abstractions.Services.Storage;
using Mini_ECommerce.Application.Helpers;
using Mini_ECommerce.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mini_ECommerce.Infrastructure.Concretes.Services.Storage
{
    // Storage service is general service that client uses, which abstracts our service provider (Azure, AWS, Local etc.)
    public class StorageService : IStorageService
    {
        private readonly IStorage _storage;

        public StorageService(IStorage storage)
        {
            _storage = storage;
        }

        public StorageType StorageName
        {
            get
            {
                var name = _storage.GetType().Name.Replace("Storage", string.Empty);
                if (EnumHelpers.TryParseEnum(name, out StorageType storageType))
                {
                    return storageType;
                }

                else throw new InvalidOperationException("Cannot parse enum");
            }
        }

        public async Task DeleteAllAsync(string pathOrContainerName)
          => await _storage.DeleteAllAsync(pathOrContainerName);


        public async Task DeleteAsync(string pathOrContainerName, string fileName)
            => await _storage.DeleteAsync(pathOrContainerName, fileName);

        public Task<List<(string fileName, string pathOrContainerName)>> GetFilesAsync(string pathOrContainerName)
            => _storage.GetFilesAsync(pathOrContainerName);

        public Task<bool> HasFileAsync(string pathOrContainerName, string fileName)
            => _storage.HasFileAsync(pathOrContainerName, fileName);

        public Task<List<(string fileName, string pathOrContainerName)>> UploadAsync(string pathOrContainerName, IFormFileCollection files)
            => _storage.UploadAsync(pathOrContainerName, files);

        public Task<bool> DeleteFileUsingUrl(string url)
        {
            throw new NotImplementedException();
        }

        public Task<string> GetFileUrl(string pathName)
        {
            throw new NotImplementedException();
        }
    }
}
