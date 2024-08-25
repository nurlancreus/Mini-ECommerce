using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mini_ECommerce.Application.Features.Commands.ProductImageFile.UpdateProductImage
{
    public class UpdateProductImageCommandRequest : IRequest<UpdateProductImageCommandResponse>
    {
        public string MainImageId { get; set; }
        public string ProductId { get; set; }
    }
}
