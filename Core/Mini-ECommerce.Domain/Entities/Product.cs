using Mini_ECommerce.Domain.Entities.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mini_ECommerce.Domain.Entities
{
    public class Product : BaseEntity
    {
        public string Name { get; set; } = "";
        public int Stock { get; set; }
        public float Price { get; set; }
        public ICollection<Order> Orders { get; set; } = [];
        public ICollection<ProductImageFile> ProductImageFiles { get; set; } = [];
        public ICollection<ProductProductImageFile> ProductProductImageFiles { get; set; } = [];

    }
}
