using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mini_ECommerce.Application.Abstractions.Services.Storage
{
    public interface IStorage
    {
        Task<List<(string fileName, string pathOrContainerName)>> UploadAsync(string pathOrContainerName, IFormFileCollection formFiles);
        Task DeleteAsync(string pathOrContainerName, string fileName);
        Task DeleteAllAsync(string pathOrContainerName);
        Task<List<(string fileName, string pathOrContainerName)>> GetFilesAsync(string pathOrContainerName);
        Task<bool> HasFileAsync(string pathOrContainerName, string fileName);
        public Task<string> GetFileUrl(string pathName);
        public Task<bool> DeleteFileUsingUrl(string url);
    }
}
