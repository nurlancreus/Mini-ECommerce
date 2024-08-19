using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mini_ECommerce.Application.Features.Queries.ProductImageFile.GetProductImages
{
    public class GetProductImagesQueryRequest : IRequest<GetProductImagesQueryResponse>
    {
        public string Id { get; set; }
    }
}
