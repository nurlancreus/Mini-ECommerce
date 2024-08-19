using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mini_ECommerce.Application.Features.Commands.ProductImageFile.UploadProductImage
{
    public class UploadProductImageCommandResponse : IRequest<UploadProductImageCommandRequest>
    {
        public string? Message { get; set; }
        ICollection<Domain.Entities.ProductImageFile> ProductImages { get; set; }
    }
}
