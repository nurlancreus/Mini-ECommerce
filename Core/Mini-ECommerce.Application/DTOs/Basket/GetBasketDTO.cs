using Mini_ECommerce.Application.DTOs.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mini_ECommerce.Application.DTOs.Basket
{
    public class GetBasketDTO
    {
        public string Id { get; set; }
        public GetAppUserDTO User { get; set; }
        public List<GetBasketItemDTO> BasketItems { get; set; } = [];
    }
}
