using Mini_ECommerce.Application.ViewModels.Product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mini_ECommerce.Application.ViewModels.ProductImageFile
{
    public class GetProductImageFileVM
    {
        public Guid Id { get; set; }
        public bool IsMain { get; set; }
        public string FileName { get; set; }
        public string Path { get; set; }
        public DateTime CreatedAt { get; set; }
        public ICollection<GetProductVM> Products { get; set; }
    }
}
