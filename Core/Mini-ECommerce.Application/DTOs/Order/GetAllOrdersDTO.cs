using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mini_ECommerce.Application.DTOs.Order
{
    public class GetAllOrdersDTO
    {
        public List<GetOrderDTO> Orders { get; set; } = [];
        public int TotalCount {  get; set; }
        public int Page {  get; set; }
        public int TotalPages { get; set; }

    }
}
