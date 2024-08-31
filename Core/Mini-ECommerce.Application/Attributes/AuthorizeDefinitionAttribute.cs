using Mini_ECommerce.Application.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mini_ECommerce.Application.Attributes
{
    public class AuthorizeDefinitionAttribute : Attribute
    {
        public AuthorizedMenu Menu { get; set; }
        public string Definition { get; set; }
        public ActionType ActionType { get; set; }
        public Role[] Roles { get; set; } = [];

    }
}
