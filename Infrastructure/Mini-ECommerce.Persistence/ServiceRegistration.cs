using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Mini_ECommerce.Application.Abstractions.Repositories.Customer;
using Mini_ECommerce.Application.Abstractions.Repositories.Order;
using Mini_ECommerce.Application.Abstractions.Repositories.Product;
using Mini_ECommerce.Persistence.Concretes.Repositories.Customer;
using Mini_ECommerce.Persistence.Concretes.Repositories.Order;
using Mini_ECommerce.Persistence.Concretes.Repositories.Product;
using Mini_ECommerce.Persistence.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Authentication.ExtendedProtection;
using System.Text;
using System.Threading.Tasks;

namespace Mini_ECommerce.Persistence
{
    public static class ServiceRegistration
    {
        public static void AddPersistenceServices (this IServiceCollection services)
        {
            services.AddDbContext<MiniECommerceDbContext>(options => options.UseNpgsql(Configuration.ConnectionString));

            services.AddScoped<ICustomerReadRepository, CustomerReadRepository>();
            services.AddScoped<ICustomerWriteRepository, CustomerWriteRepository>();
            services.AddScoped<IOrderReadRepository, OrderReadRepository>();
            services.AddScoped<IOrderWriteRepository, OrderWriteRepository>();
            services.AddScoped<IProductReadRepository, ProductReadRepository>();
            services.AddScoped<IProductWriteRepository, ProductWriteRepository>();
        }
    }
}
