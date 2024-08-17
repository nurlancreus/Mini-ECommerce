using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mini_ECommerce.Domain.Entities
{
    // TPH
    public class ProductImageFile : AppFile
    {
        public ICollection<Product> Products { get; set; }
    }
}
