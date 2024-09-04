using Mini_ECommerce.Application.Abstractions.Services;
using QRCoder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mini_ECommerce.Infrastructure.Concretes.Services
{
    public class QRCodeService : IQRCodeService
    {
        public async Task<byte[]> GenerateQRCodeAsync(string text)
        {
            QRCodeGenerator generator = new();
            QRCodeData data = generator.CreateQrCode(text, QRCodeGenerator.ECCLevel.Q);
            PngByteQRCode qrCode = new(data);
            byte[] byteGraphic = qrCode.GetGraphic(10, "TcG"u8.ToArray(), [240, 240, 240]);

            return await Task.FromResult(byteGraphic);
        }
    }
}
