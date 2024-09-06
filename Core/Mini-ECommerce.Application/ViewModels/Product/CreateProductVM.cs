using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mini_ECommerce.Application.ViewModels.Product
{
    public class CreateProductVM
    {
        public string Name { get; set; }
        public int Stock { get; set; }
        public float Price { get; set; }
        public FormFileCollection ProductImages { get; set; } = [];
    }
}
