using Mini_ECommerce.Application.Abstractions.Repositories.Customer;
using Mini_ECommerce.Persistence.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mini_ECommerce.Persistence.Concretes.Repositories.Customer
{
    public class CustomerReadRepository : ReadRepository<Domain.Entities.Customer>, ICustomerReadRepository
    {
        public CustomerReadRepository(MiniECommerceDbContext context) : base(context)
        {
        }
    }
}
