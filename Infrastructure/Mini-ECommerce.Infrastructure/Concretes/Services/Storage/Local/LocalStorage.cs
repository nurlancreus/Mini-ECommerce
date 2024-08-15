﻿using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Mini_ECommerce.Application.Abstractions.Services.Storage.Local;
using Mini_ECommerce.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mini_ECommerce.Infrastructure.Concretes.Services.Storage.Local
{
    public class LocalStorage : ILocalStorage
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        public LocalStorage(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task DeleteAsync(string path, string fileName)
        {

            if (HasFile(path, fileName))
                File.Delete(Path.Combine(path, fileName));

            await Task.CompletedTask;
        }


        public List<string> GetFiles(string path)
        {
            DirectoryInfo directory = new(path);
            return directory.GetFiles().Select(f => f.Name).ToList();
        }

        public bool HasFile(string path, string fileName)
            => File.Exists(Path.Combine(path, fileName));

        private static async Task<bool> CopyFileAsync(string path, IFormFile formFile)
        {
            try
            {
                await using FileStream fileStream = new(path, FileMode.Create, FileAccess.Write, FileShare.None, 1024 * 1024, useAsync: true);
                await formFile.CopyToAsync(fileStream);
                await fileStream.FlushAsync();
                return true;
            }
            catch (Exception ex)
            {
                // Log the exception details for debugging
                throw new Exception ($"Error copying file: {ex.Message}");
                // return false;
            }
        }

        public async Task<List<(string fileName, string pathOrContainerName)>> UploadAsync(string path, IFormFileCollection formFiles)
        {
            if (formFiles.Count == 0)
            {
                throw new ArgumentException("No files uploaded.");
            }

            string uploadPath = Path.Combine(_webHostEnvironment.WebRootPath, path);

            await FileHelpers.EnsureDirectoryExists(uploadPath);

            var uploadResults = new List<(string fileName, string path)>();

            foreach (IFormFile formFile in formFiles)
            {
                string newFileName = await FileHelpers.RenameFileAsync(path, formFile.FileName, _webHostEnvironment.WebRootPath);
                string fullPath = Path.Combine(uploadPath, newFileName);

                bool isCopied = await CopyFileAsync(fullPath, formFile);
                if (!isCopied)
                {
                    // Optionally, you could delete all uploaded files in case of failure.
                    await FileHelpers.CleanupFailedUploads(uploadResults);
                    throw new Exception("File upload failed.");
                }

                uploadResults.Add((newFileName, path));
            }

            return uploadResults;
        }

    }
}
