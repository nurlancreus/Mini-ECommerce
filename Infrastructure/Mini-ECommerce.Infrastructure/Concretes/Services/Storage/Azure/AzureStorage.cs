﻿using Microsoft.AspNetCore.Http;
using Mini_ECommerce.Application.Abstractions.Services.Storage;
using Mini_ECommerce.Application.Abstractions.Services.Storage.Azure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mini_ECommerce.Infrastructure.Concretes.Services.Storage.Azure
{
    public class AzureStorage : IAzureStorage
    {
        public Task DeleteAsync(string pathOrContainerName, string fileName)
        {
            throw new NotImplementedException();
        }

        public Task<List<(string fileName, string pathOrContainerName)>> UploadAsync(string pathOrContainerName, IFormFileCollection formFiles)
        {
            throw new NotImplementedException();
        }

        Task<List<string>> IStorage.GetFilesAsync(string pathOrContainerName)
        {
            throw new NotImplementedException();
        }

        Task<bool> IStorage.HasFileAsync(string pathOrContainerName, string fileName)
        {
            throw new NotImplementedException();
        }
    }
}
