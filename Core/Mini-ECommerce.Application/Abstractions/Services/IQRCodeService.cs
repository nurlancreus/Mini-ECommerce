using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mini_ECommerce.Application.Abstractions.Services
{
    public interface IQRCodeService
    {
        Task<byte[]> GenerateQRCodeAsync(string text);
    }
}
