using Mini_ECommerce.Domain.Enums;
using Mini_ECommerce.Domain.Entities.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mini_ECommerce.Domain.Entities.Identity;

namespace Mini_ECommerce.Domain.Entities
{
    public class AppEndpoint : BaseEntity
    {
        public AuthorizedMenu Menu { get; set; }
        public ActionType Action { get; set; }
        public MethodType HttpMethod { get; set; }
        public string Code { get; set; }
        public string Definition { get; set; }

        public ICollection<AppRole> Roles { get; set; } = [];

    }
}
