﻿using MediatR;
using Mini_ECommerce.Application.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mini_ECommerce.Application.Features.Commands.Basket.AddItemToBasket
{
    public class AddItemToBasketCommandResponse : BaseResponse, IRequest<AddItemToBasketCommandRequest>
    {

    }
}
