using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mini_ECommerce.Application.DTOs.User
{
    public class RegisterUserResponseDTO
    {
        public bool Success { get; set; }
        public string? Message { get; set; }
    }
}
