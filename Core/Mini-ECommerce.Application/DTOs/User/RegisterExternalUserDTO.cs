using Microsoft.AspNetCore.Identity;
using Mini_ECommerce.Domain.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mini_ECommerce.Application.DTOs.User
{
    public class RegisterExternalUserDTO
    {
        public AppUser User { get; set; }
        public UserLoginInfo UserInfo { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
    }
}
