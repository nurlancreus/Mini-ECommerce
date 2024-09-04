using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Mini_ECommerce.Application.Abstractions.Repositories;
using Mini_ECommerce.Application.Abstractions.Services;
using Mini_ECommerce.Application.Abstractions.Services.Auth;
using Mini_ECommerce.Domain.Entities.Identity;
using Mini_ECommerce.Persistence.Concretes.Repositories;
using Mini_ECommerce.Persistence.Concretes.Services;
using Mini_ECommerce.Persistence.Concretes.Services.Auth;
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
        public static IServiceCollection AddPersistenceServices (this IServiceCollection services, IConfiguration configuration)
        {
            //services.AddDbContext<MiniECommerceDbContext>(options => options.UseNpgsql(Configuration.ConnectionString));
           
            services.AddDbContext<MiniECommerceDbContext>(options => options.UseNpgsql(configuration.GetConnectionString("Default")));

            services.AddScoped<IOrderReadRepository, OrderReadRepository>();
            services.AddScoped<IOrderWriteRepository, OrderWriteRepository>();
            
            services.AddScoped<ICompletedOrderReadRepository, CompletedOrderReadRepository>();
            services.AddScoped<ICompletedOrderWriteRepository, CompletedOrderWriteRepository>();

            services.AddScoped<IProductReadRepository, ProductReadRepository>();
            services.AddScoped<IProductWriteRepository, ProductWriteRepository>();

            services.AddScoped<IAppFileReadRepository, AppFileReadRepository>();
            services.AddScoped<IAppFileWriteRepository, AppFileWriteRepository>();

            services.AddScoped<IBasketReadRepository, BasketReadRepository>();
            services.AddScoped<IBasketWriteRepository, BasketWriteRepository>();
            
            services.AddScoped<IBasketItemReadRepository, BasketItemReadRepository>();
            services.AddScoped<IBasketItemWriteRepository, BasketItemWriteRepository>();

            services.AddScoped<IProductImageFileReadRepository, ProductImageFileReadRepository>();
            services.AddScoped<IProductImageFileWriteRepository, ProductImageFileWriteRepository>();

            services.AddScoped<IInvoiceFileReadRepository, InvoiceFileReadRepository>();
            services.AddScoped<IInvoiceFileWriteRepository, InvoiceFileWriteRepository>(); 

            services.AddScoped<IAppEndpointReadRepository, AppEndpointReadRepository>();
            services.AddScoped<IAppEndpointWriteRepository, AppEndpointWriteRepository>();

            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IRoleService, RoleService>();

            services.AddScoped<IAuthEndpointService, AuthEndpointService>();

            services.AddScoped<IAuthManagementService, AuthService>();
            services.AddScoped<IExternalAuthService, AuthService>();
            services.AddScoped<IInternalAuthService, AuthService>();

            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<IBasketService, BasketService>();
            services.AddScoped<IOrderService, OrderService>();

            services.AddIdentity<AppUser, AppRole>()
                    .AddEntityFrameworkStores<MiniECommerceDbContext>()
                    .AddDefaultTokenProviders();

            services.Configure<IdentityOptions>(options =>
            {
                // Password settings.
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequiredLength = 4;
                options.Password.RequiredUniqueChars = 0;

                // Lockout settings.
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.AllowedForNewUsers = true;

                // User settings.
                options.User.AllowedUserNameCharacters =
                "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
                options.User.RequireUniqueEmail = true;
            });

            return services;
        }
    }
}
