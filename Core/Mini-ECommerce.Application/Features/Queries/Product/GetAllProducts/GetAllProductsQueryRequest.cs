using MediatR;
using Mini_ECommerce.Application.RequestParameters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mini_ECommerce.Application.Features.Queries.Product.GetAllProduct
{
    public class GetAllProductsQueryRequest : PaginationParams, IRequest<GetAllProductsQueryResponse>
    {
        public override int PageSize { get; set; } = 10;
    }
}
