// File: /tests/Mini_ECommerce.IntegrationTests/Controllers/UsersControllerTests.cs

using System.Net.Http;
using System.Threading.Tasks;
using Xunit;
using Mini_ECommerce.Application.Features.Commands.User.RegisterUser;
using System.Net.Http.Json;
using Mini_ECommerce.Application.Features.Commands.User.UpdatePassword;
using Mini_ECommerce.Application.Features.Queries.User.GetAllUsers;
using Mini_ECommerce.Application.Features.Commands.User.AssignRoleToUser;

namespace Mini_ECommerce.IntegrationTests.Controllers
{
    public class UsersControllerTests : IClassFixture<MiniECommerceWebApplicationFactory>
    {
        private readonly HttpClient _client;

        // Change constructor to public
        public UsersControllerTests(MiniECommerceWebApplicationFactory factory)
        {
            _client = factory.CreateClient();
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
            Assert.Contains("Password updated successfully", content); // Adjust assertion based on actual response content
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
            Assert.Contains("users", content); // Adjust assertion based on actual response content
        }

        [Fact]
        public async Task AssignRoleToUser_ShouldReturnOk_WhenRoleIsAssignedSuccessfully()
        {
            // Arrange
            var assignRoleToUserCommand = new AssignRoleToUserCommandRequest
            {
                Id = "user-id",
                Roles = ["Admin"]
            };

            // Act
            var response = await _client.PostAsJsonAsync("/api/users/assign-role-to-user", assignRoleToUserCommand);

            // Assert
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            Assert.NotNull(content);
            Assert.Contains("Role assigned successfully", content); // Adjust assertion based on actual response content
        }
    }
}
