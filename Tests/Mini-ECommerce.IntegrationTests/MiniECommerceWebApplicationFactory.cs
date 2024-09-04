using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Mini_ECommerce.API;
using Mini_ECommerce.IntegrationTests;
using Mini_ECommerce.Persistence.Contexts;
using System.Linq;

public class MiniECommerceWebApplicationFactory : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureTestServices(services =>
        {
            // Remove the existing DbContextOptions
            var descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<MiniECommerceDbContext>));
            if (descriptor != null)
            {
                services.Remove(descriptor);
            }

            // Register a new DBContext that will use our test connection string
            string? connString = GetConnectionString();
            services.AddDbContext<MiniECommerceDbContext>(options =>
                options.UseNpgsql(connString)); // Assuming you're using Npgsql

            // Add the authentication handler
            services.AddAuthentication(defaultScheme: "TestScheme")
                .AddScheme<AuthenticationSchemeOptions, TestAuthHandler>(
                    "TestScheme", options => { });

            // Ensure the database is created and migrated
            using var serviceProvider = services.BuildServiceProvider();
            using var scope = serviceProvider.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<MiniECommerceDbContext>();
            dbContext.Database.EnsureDeleted(); // Ensure the database is deleted if it exists
            dbContext.Database.Migrate(); // Apply migrations
        });
    }

    private static string? GetConnectionString()
    {
        var configuration = new ConfigurationBuilder()
            .AddUserSecrets<MiniECommerceWebApplicationFactory>()
            .Build();

        var connString = configuration.GetConnectionString("DefaultTest");
        if (string.IsNullOrEmpty(connString))
        {
            throw new InvalidOperationException("Test connection string is not set in user secrets.");
        }

        return connString;
    }
}
