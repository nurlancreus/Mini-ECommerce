
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

namespace Mini_ECommerce.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Apply the logging configuration
            builder.ConfigureLogging();

            builder.Services.AddRouting(options => options.LowercaseUrls = true);

            builder.Services.AddStorage(StorageType.AWS, builder.Configuration);

            // Add services to the container.
            builder.Services.AddApplicationServices()
                .AddPersistenceServices(builder.Configuration)
                .AddInfrastructureServices();

            builder.Services.AddControllers();

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
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Your API", Version = "v1" });

                // Define the BearerAuth scheme
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });

                // Define the Google Auth scheme
                c.AddSecurityDefinition("Google", new OpenApiSecurityScheme
                {
                    Description = "Google OAuth2.0",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.OAuth2,
                    Flows = new OpenApiOAuthFlows
                    {
                        AuthorizationCode = new OpenApiOAuthFlow
                        {
                            AuthorizationUrl = new Uri("https://accounts.google.com/o/oauth2/auth"),
                            TokenUrl = new Uri("https://oauth2.googleapis.com/token"),
                            Scopes = new Dictionary<string, string>
                {
                    { "openid", "Access to your Google account" },
                    { "profile", "Access to your Google profile" },
                    { "email", "Access to your email" }
                }
                        }
                    }
                });

                // Apply the BearerAuth and GoogleAuth schemes to all API endpoints
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        },
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Google"
                }
            },
            Array.Empty<string>()
        }
    });
            });

            var app = builder.Build();

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

            // should put above everything you want to log (only logs the things coming after itself)
            app.UseSerilogRequestLogging();

            app.UseStaticFiles();

            // for http logging
            app.UseHttpLogging();

            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();

            // should put after UseAuth* middlewares
            app.UseMyLoggingMiddleware();

            app.MapControllers();

            app.Run();
        }
    }
}
