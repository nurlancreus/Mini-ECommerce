using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mini_ECommerce.Application.Abstractions.Services
{
    public interface IFileService
    {
        Task<List<(string fileName, string path)>> UploadAsync (string path, IFormFileCollection formFiles);
        Task<bool> CopyFileAsync(string path, IFormFile formFile);
    }
}
