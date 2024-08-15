using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Mini_ECommerce.Application.Abstractions.Repositories;
using Mini_ECommerce.Application.Abstractions.Repositories;
using Mini_ECommerce.Application.Abstractions.Repositories;
using Mini_ECommerce.Persistence.Concretes.Repositories;
using Mini_ECommerce.Persistence.Concretes.Repositories;
using Mini_ECommerce.Persistence.Concretes.Repositories;
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
        public static void AddPersistenceServices (this IServiceCollection services, IConfiguration configuration)
        {
            //services.AddDbContext<MiniECommerceDbContext>(options => options.UseNpgsql(Configuration.ConnectionString));
           
            services.AddDbContext<MiniECommerceDbContext>(options => options.UseNpgsql(configuration.GetConnectionString("PostgreSQL")));

            services.AddScoped<ICustomerReadRepository, CustomerReadRepository>();
            services.AddScoped<ICustomerWriteRepository, CustomerWriteRepository>();
            services.AddScoped<IOrderReadRepository, OrderReadRepository>();
            services.AddScoped<IOrderWriteRepository, OrderWriteRepository>();
            services.AddScoped<IProductReadRepository, ProductReadRepository>();
            services.AddScoped<IProductWriteRepository, ProductWriteRepository>();
        }
    }
}
