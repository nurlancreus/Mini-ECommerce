using Microsoft.Extensions.DependencyInjection;
using Mini_ECommerce.Application.Abstractions.Services;
using Mini_ECommerce.Application.Abstractions.Services.Storage;
using Mini_ECommerce.Infrastructure.Concretes.Services;
using Mini_ECommerce.Infrastructure.Concretes.Services.Storage.Local;
using Mini_ECommerce.Infrastructure.Concretes.Services.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mini_ECommerce.Domain.Enums;
using Microsoft.Extensions.Configuration;
using Amazon.S3;
using Mini_ECommerce.Infrastructure.Concretes.Services.Storage.AWS;
using Mini_ECommerce.Application.Abstractions.Services.Token;
using Mini_ECommerce.Infrastructure.Concretes.Services.Token;

namespace Mini_ECommerce.Infrastructure
{
    public static class ServiceRegistration
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
        {
            // services.AddScoped<IFileService, FileService>();
            services.AddScoped<IStorageService, StorageService>();
            services.AddScoped<IAppTokenHandler, AppTokenHandler>();
            services.AddScoped<IPaginationService, PaginationService>();

            return services;
        }

        //public static void AddStorage<T>(this IServiceCollection services) where T : class, IStorage
        //{
        //    services.AddScoped<IStorage, T>();
        //}

        public static void AddStorage(this IServiceCollection services, StorageType storageType, IConfiguration configuration)
        {
            switch (storageType)
            {
                case StorageType.Local:
                    services.AddScoped<IStorage, LocalStorage>();
                    break;

                case StorageType.Azure:
                    // Uncomment and configure for Azure Storage
                    // services.AddScoped<IStorage, AzureStorage>();
                    break;

                case StorageType.AWS:
                    ConfigureAWSServices(services, configuration);
                    break;

                default:
                    services.AddScoped<IStorage, LocalStorage>();
                    break;
            }
        }

        private static void ConfigureAWSServices(IServiceCollection services, IConfiguration configuration)
        {
            var awsOptions = configuration.GetAWSOptions();
            awsOptions.Credentials = new Amazon.Runtime.BasicAWSCredentials(
                configuration["Storage:AWS:AccessKey"],
                configuration["Storage:AWS:SecretAccessKey"]);
            awsOptions.Region = Amazon.RegionEndpoint.GetBySystemName(configuration["Storage:AWS:Region"]); // Ensure the region is set


            services.AddDefaultAWSOptions(awsOptions);
            services.AddAWSService<IAmazonS3>();

            services.AddScoped<IStorage, AWSStorage>();
        }
    }
}
