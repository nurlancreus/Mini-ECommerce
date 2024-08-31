using Microsoft.AspNetCore.Identity;
using Mini_ECommerce.Domain.Entities.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mini_ECommerce.Domain.Entities.Identity
{
    public class AppRole : IdentityRole, IBase
    {
    }
}
