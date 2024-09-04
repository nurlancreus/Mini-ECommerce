using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mini_ECommerce.Application.RequestParameters
{
    public class PaginationParams
    {
        public virtual int Page { get; set; } = 1;
        public virtual int PageSize { get; set; } = 5;
    }
}
