using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mini_ECommerce.Domain.Entities.Base
{
    public abstract class BaseEntity : IBase
    {
        public Guid Id { get; set; }
        public DateTime CreatedAt { get; set; }
        virtual public DateTime? UpdatedAt { get; set; }
    }
}
