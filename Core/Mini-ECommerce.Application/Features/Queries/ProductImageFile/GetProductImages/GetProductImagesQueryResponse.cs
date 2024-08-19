using MediatR;
using Mini_ECommerce.Application.ViewModels.ProductImageFile;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mini_ECommerce.Application.Features.Queries.ProductImageFile.GetProductImages
{
    public class GetProductImagesQueryResponse : IRequest<GetProductImagesQueryRequest>
    {
        public ICollection<GetProductImageFileVM> Images { get; set; } = [];
    }
}
