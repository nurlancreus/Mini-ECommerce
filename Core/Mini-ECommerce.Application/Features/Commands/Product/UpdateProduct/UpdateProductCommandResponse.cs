using MediatR;
using Mini_ECommerce.Application.ViewModels.Product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mini_ECommerce.Application.Features.Commands.Product.UpdateProduct
{
    public class UpdateProductCommandResponse : IRequest<UpdateProductCommandRequest>
    {
        public GetProductVM Product {  get; set; }
        public string? Message { get; set; }

    }
}
