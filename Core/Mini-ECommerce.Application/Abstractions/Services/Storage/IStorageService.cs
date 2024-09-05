using Mini_ECommerce.Application.Abstractions.Services.Storage.AWS;
using Mini_ECommerce.Application.Abstractions.Services.Storage.Azure;
using Mini_ECommerce.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mini_ECommerce.Application.Abstractions.Services.Storage
{
    public interface IStorageService : IStorage
    {
        public StorageType StorageName { get; }
    }
}
