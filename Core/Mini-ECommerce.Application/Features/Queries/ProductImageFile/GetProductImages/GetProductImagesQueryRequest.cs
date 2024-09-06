using MediatR;
using Mini_ECommerce.Application.RequestParameters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mini_ECommerce.Application.Features.Queries.ProductImageFile.GetProductImages
{
    public class GetProductImagesQueryRequest : PaginationParams, IRequest<GetProductImagesQueryResponse>
    {
        public string Id { get; set; }
    }
}
