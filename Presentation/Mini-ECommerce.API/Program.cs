
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
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
    });

            builder.Services.AddAuthorization();

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
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
