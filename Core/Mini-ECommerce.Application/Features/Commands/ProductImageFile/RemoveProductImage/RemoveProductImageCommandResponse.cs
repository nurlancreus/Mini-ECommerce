using MediatR;
using Mini_ECommerce.Application.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mini_ECommerce.Application.Features.Commands.ProductImageFile.RemoveProductImage
{
    public class RemoveProductImageCommandResponse : BaseResponse, IRequest<RemoveProductImageCommandRequest>
    {
    }
}
