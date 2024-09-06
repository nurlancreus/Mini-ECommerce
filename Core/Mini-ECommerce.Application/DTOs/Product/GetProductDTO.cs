using Mini_ECommerce.Application.DTOs.File;
using Mini_ECommerce.Application.ViewModels.File;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mini_ECommerce.Application.DTOs.Product
{
    public class GetProductDTO
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public int Stock { get; set; }
        public float Price { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public List<GetProductImageFileDTO> ProductImageFiles { get; set; } = [];
    }
}
