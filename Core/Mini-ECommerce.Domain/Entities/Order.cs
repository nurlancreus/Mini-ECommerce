﻿using Mini_ECommerce.Domain.Entities.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mini_ECommerce.Domain.Entities
{
    public class Order : BaseEntity
    {
        public string Description { get; set; }
        // public string Address { get; set; }
        public string OrderCode { get; set; }
        public Address Address { get; set; }
        public Basket Basket { get; set; }
        public CompletedOrder CompletedOrder { get; set; }
        // public ICollection<Product> Products { get; set; } = [];
    }
}
