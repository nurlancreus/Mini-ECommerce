using Mini_ECommerce.Application.ViewModels.Product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mini_ECommerce.Application.ViewModels.File
{
    public class GetProductImageFileVM : GetAppFileVM
    {
        public bool IsMain { get; set; }
        public ICollection<GetProductVM> Products { get; set; }
    }
}
