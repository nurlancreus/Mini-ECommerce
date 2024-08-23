
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Mini_ECommerce.Application;
using Mini_ECommerce.Application.Validators.Product;
using Mini_ECommerce.Domain.Enums;
using Mini_ECommerce.Infrastructure;
using Mini_ECommerce.Infrastructure.Concretes.Services.Storage.Local;
using Mini_ECommerce.Infrastructure.Filters;
using Mini_ECommerce.Persistence;
using System.Security.Claims;
using System.Text;

namespace Mini_ECommerce.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddRouting(options => options.LowercaseUrls = true);

            builder.Services.AddStorage(StorageType.AWS, builder.Configuration);

            // Add services to the container.
            builder.Services.AddApplicationServices();
            builder.Services.AddPersistenceServices(builder.Configuration);
            builder.Services.AddInfrastructureServices();

            builder.Services.AddControllers();

            // builder.Services.AddControllers(options => options.Filters.Add<ValidationFilter>());

            //builder.Services.AddFluentValidation(configuration => configuration.RegisterValidatorsFromAssemblyContaining<CreateProductValidator>());

            builder.Services//AddFluentValidationAutoValidation()
                .AddFluentValidationClientsideAdapters();

            builder.Services.AddValidatorsFromAssemblyContaining<CreateProductCommandValidator>();

            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer("Admin", options =>
    {
        options.TokenValidationParameters = new()
        {
            ValidateAudience = true, // Specifies which origins/sites can use the generated token value. -> www.somesite.com
            ValidateIssuer = true, // Specifies who issued the generated token value. -> www.myapi.com
            ValidateIssuerSigningKey = true, // Validation of the security key, confirming that the generated token value belongs to our application.
            ValidateLifetime = true, // Validation that checks the expiration time of the generated token value.

            ValidAudience = builder.Configuration["Token:Audience"],
            ValidIssuer = builder.Configuration["Token:Issuer"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Token:SecurityKey"])),
            LifetimeValidator = (notBefore, expires, securityToken, validationParameters) => expires != null && expires > DateTime.UtcNow,

            NameClaimType = ClaimTypes.Name // The value corresponding to the Name claim in the JWT can be obtained from the User.Identity.Name property.
        };
    }).AddGoogle(options =>
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

            app.UseStaticFiles();
            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
