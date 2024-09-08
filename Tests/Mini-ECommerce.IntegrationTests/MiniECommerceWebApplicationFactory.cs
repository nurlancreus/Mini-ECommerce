using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Mini_ECommerce.API;
using Mini_ECommerce.Persistence.Contexts;
using System.Linq;
using Testcontainers.PostgreSql;
using Microsoft.Extensions.Configuration;
using Mini_ECommerce.IntegrationTests;
using Microsoft.AspNetCore.Authorization.Policy;

public class MiniECommerceWebApplicationFactory : WebApplicationFactory<Program>
{
    private PostgreSqlContainer _postgreSqlContainer;

    // Setup the Testcontainer for PostgreSQL
    public MiniECommerceWebApplicationFactory()
    {
        _postgreSqlContainer = new PostgreSqlBuilder()
            .WithDatabase("MiniECommerceDbTest")
            .WithUsername("mypostgres")
            .WithPassword("mysecretpassword")
            .Build();

        _postgreSqlContainer.StartAsync().GetAwaiter().GetResult(); // Start the container
    }

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

            // Register the new DbContext using Testcontainer's connection string
            string connString = _postgreSqlContainer.GetConnectionString();
            services.AddDbContext<MiniECommerceDbContext>(options =>
                options.UseNpgsql(connString));

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

    public override async ValueTask DisposeAsync()
    {
        // Stop and dispose of the PostgreSQL container when tests are done
        await _postgreSqlContainer.StopAsync();
        await _postgreSqlContainer.DisposeAsync();
        await base.DisposeAsync();
    }
}
