using Mini_ECommerce.Application.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mini_ECommerce.Application.DTOs.Configuration
{
    public class ActionDTO
    {
        public string ActionType { get; set; }
        public string Method { get; set; }
        public string Definition { get; set; }
        public string Code { get; set; }
        public string[] Roles { get; set; }
    }
}
