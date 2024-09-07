﻿using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mini_ECommerce.Application.Features.Commands.ProductImageFile.ChangeMainImage
{
    public class ChangeMainImageCommandRequest : IRequest<ChangeMainImageCommandResponse>
    {
        public string ProductId { get; set; }
        public string ProductImageId { get; set; }
    }
}
