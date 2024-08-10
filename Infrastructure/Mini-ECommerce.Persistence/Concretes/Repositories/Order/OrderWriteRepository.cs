using Mini_ECommerce.Application.Abstractions.Repositories.Order;
using Mini_ECommerce.Persistence.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mini_ECommerce.Persistence.Concretes.Repositories.Order
{
    public class OrderWriteRepository : WriteRepository<Domain.Entities.Order>, IOrderWriteRepository
    {
        public OrderWriteRepository(MiniECommerceDbContext context) : base(context)
        {
        }
    }
}
