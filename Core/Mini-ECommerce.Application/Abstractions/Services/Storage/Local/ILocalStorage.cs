using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Mini_ECommerce.Application.Abstractions.Services.Storage;

namespace Mini_ECommerce.Application.Abstractions.Services.Storage.Local
{
    public interface ILocalStorage : IStorage
    {
        //protected Task<bool> CopyFileAsync(string path, IFormFile formFile);
    }
}
