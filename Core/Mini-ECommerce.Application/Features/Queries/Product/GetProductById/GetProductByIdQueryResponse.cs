using MediatR;
using Mini_ECommerce.Application.Responses;
using Mini_ECommerce.Application.ViewModels.Product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mini_ECommerce.Application.Features.Queries.Product.GetProductById
{
    public class GetProductByIdQueryResponse : BaseResponse, IRequest<GetProductByIdQueryRequest>
    {
        public GetProductVM Product { get; set; }
    }
}
