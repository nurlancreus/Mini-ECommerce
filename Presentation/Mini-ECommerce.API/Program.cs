
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.HttpLogging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Mini_ECommerce.Application;
using Mini_ECommerce.Application.Validators.Product;
using Mini_ECommerce.Domain.Enums;
using Mini_ECommerce.Infrastructure;
using Mini_ECommerce.Infrastructure.Concretes.Services.Storage.Local;
using Mini_ECommerce.Infrastructure.Filters;
using Mini_ECommerce.Persistence;
using NpgsqlTypes;
using Serilog.Sinks.PostgreSQL;
using Serilog;
using System.Security.Claims;
using System.Text;
using Serilog.Core;
using Mini_ECommerce.API.Configurations;
using Serilog.Context;
using Mini_ECommerce.API.Middlewares;
using Mini_ECommerce.SignalR;
using ETicaretAPI.API.Filters;
using Microsoft.AspNetCore.RateLimiting;
using System.Threading.RateLimiting;

namespace Mini_ECommerce.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddCors(options => options.AddDefaultPolicy(policy =>
            policy.WithOrigins("http://localhost:4200", "https://localhost:4200")
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials()
            ));

            builder.Services.AddHttpContextAccessor(); // A service that allows us to access the HttpContext object created as a result of the client's request through the classes in the layers (business logic).


            // Apply the logging configuration
            builder.ConfigureLogging();

            builder.Services.AddRouting(options => options.LowercaseUrls = true);

            builder.Services.AddStorage(StorageType.AWS, builder.Configuration);


            // Add services to the container.
            builder.Services.AddApplicationServices(builder.Configuration)
                .AddPersistenceServices(builder.Configuration)
                .AddInfrastructureServices()
                .AddSignalRServices();

            builder.Services.AddControllers(options =>
            {   // Check role permissions
                options.Filters.Add<RolePermissionFilter>();
            });

            // builder.Services.AddControllers(options => options.Filters.Add<ValidationFilter>());

            //builder.Services.AddFluentValidation(configuration => configuration.RegisterValidatorsFromAssemblyContaining<CreateProductValidator>());

            builder.Services//AddFluentValidationAutoValidation()
                .AddFluentValidationClientsideAdapters();

            builder.Services.AddValidatorsFromAssemblyContaining<CreateProductCommandValidator>();

            builder.ConfigureAuth().AddGoogle(options =>
            {
                options.ClientId = builder.Configuration["ExternalLoginSettings:Google:ClientId"];
                options.ClientSecret = builder.Configuration["ExternalLoginSettings:Google:ClientSecret"];
            });

            builder.Services.AddAuthorization();

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();

            // Configure Swagger
            builder.ConfigureSwagger();

            // Implement built-in rate limiter
            builder.Services.AddRateLimiter(options => options
            .AddFixedWindowLimiter(policyName: "fixed", limiterOptions =>
            {
                limiterOptions.PermitLimit = 100;
                limiterOptions.Window = TimeSpan.FromMinutes(1);
                limiterOptions.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
                limiterOptions.QueueLimit = 5;
            }));

            var app = builder.Build();

            //Use rate limiter
            app.UseRateLimiter();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Your API v1");
                    c.OAuthClientId(builder.Configuration["Authentication:Google:ClientId"]);
                    c.OAuthClientSecret(builder.Configuration["Authentication:Google:ClientSecret"]);
                });
            }

            app.ConfigureExceptionHandler(app.Services.GetRequiredService<ILogger<Program>>());

            // should put above everything you want to log (only logs the things coming after itself)
            app.UseSerilogRequestLogging();

            app.UseStaticFiles();

            // for http logging
            app.UseHttpLogging();
            app.UseCors();
            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();

            // should put after UseAuth* middlewares
            app.UseMyLoggingMiddleware();

            app.MapControllers();

            // maps signalR hubs
            app.MapHubs();

            app.Run();
        }
    }
}
