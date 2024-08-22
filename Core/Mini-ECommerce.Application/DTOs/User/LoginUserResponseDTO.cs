using Mini_ECommerce.Application.DTOs.Token;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mini_ECommerce.Application.DTOs.User
{
    public class LoginUserResponseDTO
    {
        public TokenDTO Token { get; set; }
        public string? Message { get; set; }

    }
}
