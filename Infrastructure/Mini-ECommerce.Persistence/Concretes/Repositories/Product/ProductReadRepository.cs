using Mini_ECommerce.Application.Abstractions.Repositories.Product;
using Mini_ECommerce.Persistence.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mini_ECommerce.Persistence.Concretes.Repositories.Product
{
    public class ProductReadRepository : ReadRepository<Domain.Entities.Product>, IProductReadRepository
    {
        public ProductReadRepository(MiniECommerceDbContext context) : base(context)
        {
        }
    }
}
