using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Mini_ECommerce.Application.Abstractions.Services;
using Mini_ECommerce.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Mini_ECommerce.Infrastructure.Concretes.Services
{
    public class FileService : IFileService
    {
        private readonly IWebHostEnvironment _webHostEnvironment;

        public FileService(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task<bool> CopyFileAsync(string path, IFormFile formFile)
        {
            try
            {
                await using FileStream fileStream = new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.None, 1024 * 1024, useAsync: true);
                await formFile.CopyToAsync(fileStream);
                await fileStream.FlushAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<List<(string fileName, string path)>> UploadAsync(string folderPath, IFormFileCollection formFiles)
        {
            if (formFiles.Count == 0)
            {
                throw new ArgumentException("No files uploaded.");
            }

            string uploadPath = Path.Combine(_webHostEnvironment.WebRootPath, folderPath);

            await EnsureDirectoryExists(uploadPath);

            var uploadResults = new List<(string fileName, string path)>();

            foreach (IFormFile formFile in formFiles)
            {
                string newFileName = await RenameFileAsync(folderPath, formFile.FileName);
                string fullPath = Path.Combine(folderPath, newFileName);

                bool isCopied = await CopyFileAsync(fullPath, formFile);
                if (!isCopied)
                {
                    // Optionally, you could delete all uploaded files in case of failure.
                    await CleanupFailedUploads(uploadResults);
                    throw new Exception("File upload failed.");
                }

                uploadResults.Add((newFileName, fullPath));
            }

            return uploadResults;
        }

        private async static Task<string> RenameFileAsync(string path, string fileName)
        {
            string oldName = Path.GetFileNameWithoutExtension(fileName);
            string extension = Path.GetExtension(fileName);
            string newFileName = $"{NameHelpers.CharacterRegulatory(oldName)}{extension}";

            string newFilePath = Path.Combine(path, newFileName);

            int counter = 1;

            while (File.Exists(newFilePath))
            {
                newFileName = $"{NameHelpers.CharacterRegulatory(oldName)}-{++counter}{extension}";
                newFilePath = Path.Combine(path, newFileName);
            }

            return await Task.FromResult(newFileName);
        }

        private static async Task EnsureDirectoryExists(string path)
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
                await Task.CompletedTask; // Simulating async for consistency

            }
        }

        private static async Task CleanupFailedUploads(List<(string fileName, string path)> uploadedFiles)
        {
            foreach (var (_, path) in uploadedFiles)
            {
                if (File.Exists(path))
                {
                    File.Delete(path);

                    await Task.CompletedTask; // Simulating async for consistency
                }
            }
        }

        private static Task<string> GenerateUniqueFileNameAsync(string fileName)
        {
            Path.GetRandomFileName();
            string uniqueFileName = $"{Path.GetFileNameWithoutExtension(fileName)}_{Guid.NewGuid()}{Path.GetExtension(fileName)}";
            return Task.FromResult(uniqueFileName);
        }
    }
}
