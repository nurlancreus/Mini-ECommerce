using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mini_ECommerce.Application.DTOs.Token
{
    public class TokenDTO
    {
        public string AccessToken { get; set; }
        public DateTime ExpirationDate { get; set; }
        public string RefreshToken { get; set; }
    }
}
