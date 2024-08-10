using Microsoft.EntityFrameworkCore;
using Mini_ECommerce.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mini_ECommerce.Persistence.Contexts
{
    public class MiniECommerceDbContext(DbContextOptions<MiniECommerceDbContext> contextOptions) : DbContext(contextOptions)
    {
        public DbSet<Product> Products { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Order> Orders { get; set; }
    }
}
