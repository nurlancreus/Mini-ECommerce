using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mini_ECommerce.Application.Features.Queries.GetQrCodeToProduct
{
    public class GetQrCodeToProductQueryRequest : IRequest<GetQrCodeToProductQueryResponse>
    {
        public string ProductId { get; set; }
    }
}
