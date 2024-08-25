using Mini_ECommerce.Application.Abstractions.Repositories;
using Mini_ECommerce.Domain.Entities;
using Mini_ECommerce.Persistence.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mini_ECommerce.Persistence.Concretes.Repositories
{
    public class BasketItemWriteRepository : WriteRepository<BasketItem>, IBasketItemWriteRepository
    {
        public BasketItemWriteRepository(MiniECommerceDbContext context) : base(context)
        {
        }
    }
}
