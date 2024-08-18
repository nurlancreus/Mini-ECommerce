﻿using MediatR;
using Mini_ECommerce.Application.ViewModels.Product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Mini_ECommerce.Application.Features.Commands.Product.CreateProduct
{
    public class CreateProductCommandResponse : IRequest<CreateProductCommandRequest>
    {
        public GetProductVM Product {  get; set; }
        public string? Message { get; set; }
    }
}
