using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mini_ECommerce.Application.ViewModels.Product
{
    public class GetProductVM
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int Stock { get; set; }
        public float Price { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
