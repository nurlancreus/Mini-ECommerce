// File: /tests/Mini_ECommerce.IntegrationTests/Controllers/UsersControllerTests.cs

using System.Net.Http;
using System.Threading.Tasks;
using Xunit;
using Mini_ECommerce.Application.Features.Commands.User.RegisterUser;
using System.Net.Http.Json;
using Mini_ECommerce.Application.Features.Commands.User.UpdatePassword;
using Mini_ECommerce.Application.Features.Queries.User.GetAllUsers;
using Mini_ECommerce.Application.Features.Commands.User.AssignRoleToUser;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Mini_ECommerce.Persistence.Contexts;
using Mini_ECommerce.Application.Features.Commands.Role.CreateRole;

namespace Mini_ECommerce.IntegrationTests.Controllers
{
    public class UsersControllerTests : IClassFixture<MiniECommerceWebApplicationFactory>
    {
        private readonly HttpClient _client;
        private readonly IServiceScopeFactory _scopeFactory;

        public UsersControllerTests(MiniECommerceWebApplicationFactory factory)
        {
            _client = factory.CreateClient();

            // Get the scope factory to create service scopes
            _scopeFactory = factory.Services.GetRequiredService<IServiceScopeFactory>();
        }

        [Fact]
        public async Task Register_ShouldReturnOk_WhenUserIsRegisteredSuccessfully()
        {

            // Arrange
            var registerUserCommand = new RegisterUserCommandRequest
            {
                FirstName = "UserTest",
                LastName = "UserTestSurname",
                UserName = "TestUser1",
                Email = "test@example.com",
                Password = "Password123!",
                ConfirmPassword = "Password123!"
            };

            // Act
            var response = await _client.PostAsJsonAsync("/api/users/register", registerUserCommand);

            // Assert
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            Assert.NotNull(content);
            Assert.Contains("User registered successfully", content); // Adjust assertion based on actual response content
        }

        [Fact]
        public async Task UpdatePassword_ShouldReturnOk_WhenPasswordIsUpdatedSuccessfully()
        {
            // Arrange
            var updatePasswordCommand = new UpdatePasswordCommandRequest
            {
                UserId = "user-id",
                ResetToken = "",
                Password = "",
                ConfirmPassword = ""
            };

            // Act
            var response = await _client.PostAsJsonAsync("/api/users/update-password", updatePasswordCommand);

            // Assert
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            Assert.NotNull(content);
            Assert.Contains("Password updated successfully!", content); // Adjust assertion based on actual response content
        }

        [Fact]
        public async Task GetAllUsers_ShouldReturnOk_WithListOfUsers()
        {
            // Arrange
            var getAllUsersQuery = new GetAllUsersQueryRequest();

            // Act
            var response = await _client.GetAsync("/api/users");

            // Assert
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            Assert.NotNull(content);
            // Assert.Contains("users", content); // Adjust assertion based on actual response content
        }

        [Fact]
        public async Task AssignRoleToUser_ShouldReturnOk_WhenRoleIsAssignedSuccessfully()
        {
            // Arrange: Register a new user
            var registerUserCommand = new RegisterUserCommandRequest
            {
                FirstName = "UserTest",
                LastName = "UserTestSurname",
                UserName = "TestUser1",
                Email = "test@example.com",
                Password = "Password123!",
                ConfirmPassword = "Password123!"
            };

            var registerUserResponse = await _client.PostAsJsonAsync("/api/users/register", registerUserCommand);
            registerUserResponse.EnsureSuccessStatusCode();

            // Retrieve the registered user from the test database to get their ID
            using var scope = _scopeFactory.CreateScope(); // Create a new scope
            var dbContext = scope.ServiceProvider.GetRequiredService<MiniECommerceDbContext>(); // Resolve the DbContext within the scope

            // Retrieve the user from the database
            var user = await dbContext.Users.SingleOrDefaultAsync(u => u.Email == "test@example.com");
            Assert.NotNull(user); // Ensure the user was created

            var userId = user?.Id ?? throw new Exception("User not found");

            // Arrange: Create a new role
            var createRoleRequest = new CreateRoleCommandRequest
            {
                Name = "Admin",
            };

            var createRoleResponse = await _client.PostAsJsonAsync("/api/roles", createRoleRequest);
            createRoleResponse.EnsureSuccessStatusCode();

            // Act: Assign the role to the user
            var assignRoleToUserCommand = new AssignRoleToUserCommandRequest
            {
                Id = userId, // User ID from the database
                Roles = ["Admin"]
            };

            var response = await _client.PostAsJsonAsync("/api/users/assign-role-to-user", assignRoleToUserCommand);

            // Assert: Verify the role was assigned successfully
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            Assert.NotNull(content);
            Assert.Contains("User roles modified successfully!", content); // Adjust assertion based on actual response content
        }
    }
}
