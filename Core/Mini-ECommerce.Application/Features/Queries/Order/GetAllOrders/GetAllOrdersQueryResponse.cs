﻿using MediatR;
using Mini_ECommerce.Application.ViewModels.Order;
using Mini_ECommerce.Application.ViewModels.Product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mini_ECommerce.Application.Features.Queries.Order.GetAllOrders
{
    public class GetAllOrdersQueryResponse : IRequest<GetAllOrdersQueryRequest>
    {
        public List<GetOrderVM> Orders { get; set; } = [];

    }
}
