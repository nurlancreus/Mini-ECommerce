using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mini_ECommerce.Domain.Entities
{
    public class ProductProductImageFile
    {
        public Guid ProductId { get; set; }
        public Guid ProductImageFileId { get; set; }
        public Product Product { get; set; }
        public ProductImageFile ProductImageFile { get; set; }
        public bool IsMain { get; set; }
    }
}
