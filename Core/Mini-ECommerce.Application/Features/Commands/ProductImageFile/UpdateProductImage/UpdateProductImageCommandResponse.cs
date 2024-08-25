using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mini_ECommerce.Application.Features.Commands.ProductImageFile.UpdateProductImage
{
    public class UpdateProductImageCommandResponse : IRequest<UpdateProductImageCommandRequest>
    {
        public bool Success { get; set; }
        public string? Message { get; set; }
    }
}
