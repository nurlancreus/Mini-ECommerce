using Microsoft.AspNetCore.Identity;
using Mini_ECommerce.Domain.Entities.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mini_ECommerce.Domain.Entities.Identity
{
    public class AppUser : IdentityUser, IBase
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string? RefreshToken { get; set; }
        public DateTime? RefreshTokenEndDate { get; set; }
        public ICollection<Basket> Baskets { get; set; }

    }
}
