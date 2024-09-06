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
using Mini_ECommerce.Application.Options.Auth.External;
using Mini_ECommerce.Application.Options.General;
using Mini_ECommerce.Application.Options.Mail;
using Mini_ECommerce.Application.Options.Storage;
using Mini_ECommerce.Application.Options.Token;
using Microsoft.Extensions.Configuration;

namespace Mini_ECommerce.Application
{
    public static class ServiceRegistration
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddMediatR(config => {
                config.RegisterServicesFromAssembly(typeof(ServiceRegistration).Assembly);
                config.AddOpenBehavior(typeof(CustomValidationBehavior<,>));
            });


            // Register MediatR pipeline behavior for FluentValidation
            // services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

            services.AddHttpClient();

            services.Configure<ConnectionStringsOptions>(configuration.GetSection("ConnectionStrings"));
            services.Configure<StorageOptions>(configuration.GetSection("Storage"));
            services.Configure<Mini_ECommerce.Application.Options.Token.TokenOptions>(configuration.GetSection("Token"));
            services.Configure<ExternalLoginSettingsOptions>(configuration.GetSection("ExternalLoginSettings"));
            services.Configure<MailOptions>(configuration.GetSection("Mail"));
            services.Configure<SeqOptions>(configuration.GetSection("Seq"));

            return services;
        }
    }
}
