using MediatR.Extensions.FluentValidation.AspNetCore;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Mini_ECommerce.Application.Pipelines;

namespace Mini_ECommerce.Application
{
    public static class ServiceRegistration
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddMediatR(config => {
                config.RegisterServicesFromAssembly(typeof(ServiceRegistration).Assembly);
                config.AddOpenBehavior(typeof(CustomValidationBehavior<,>));
            });


            // Register MediatR pipeline behavior for FluentValidation
            // services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

            services.AddHttpClient();

            return services;
        }
    }
}
