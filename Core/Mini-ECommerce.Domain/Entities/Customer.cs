using Mini_ECommerce.Domain.Entities.Base;
using Mini_ECommerce.Domain.Entities.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mini_ECommerce.Domain.Entities
{
    public class Customer : BaseEntity
    {
        public string Name { get; set; }
        public string AppUserId { get; set; }
        public AppUser AppUser { get; set; }
        public ICollection<Order> Orders { get; set; } = [];
        
    }
}
