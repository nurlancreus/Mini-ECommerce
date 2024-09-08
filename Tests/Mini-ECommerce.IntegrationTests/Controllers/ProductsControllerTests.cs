using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Mini_ECommerce.Application.Features.Commands.Product.CreateProduct;
using Mini_ECommerce.Application.Features.Commands.Role.CreateRole;
using Mini_ECommerce.Application.Features.Commands.User.AssignRoleToUser;
using Mini_ECommerce.Application.Features.Commands.User.RegisterUser;
using Mini_ECommerce.Application.Features.Commands.User.UpdatePassword;
using Mini_ECommerce.Application.Features.Queries.User.GetAllUsers;
using Mini_ECommerce.Persistence.Contexts;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace Mini_ECommerce.IntegrationTests.Controllers
{
    public class ProductsControllerTests : IClassFixture<MiniECommerceWebApplicationFactory>
    {
        private readonly HttpClient _client;
        private readonly IServiceScopeFactory _scopeFactory;

        public ProductsControllerTests(MiniECommerceWebApplicationFactory factory)
        {
            _client = factory.CreateClient();

            // Get the scope factory to create service scopes
            _scopeFactory = factory.Services.GetRequiredService<IServiceScopeFactory>();
        }


        [Fact]
        public async Task GetAll_ShouldReturnOk_WithProducts()
        {
            // Arrange
            var request = new HttpRequestMessage(HttpMethod.Get, "/api/products");

            // Act
            var response = await _client.SendAsync(request);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var content = await response.Content.ReadAsStringAsync();
            content.Should().NotBeNullOrEmpty();
        }

        [Fact]
        public async Task Create_ShouldReturnOk_WhenProductIsCreated()
        {
            // Arrange
            var productImagesContent = new MultipartFormDataContent();

            // Create a mock file
            var fileContent = new ByteArrayContent(new byte[] { 0x20, 0x20 }); // Mock file data
            fileContent.Headers.ContentType = MediaTypeHeaderValue.Parse("image/jpeg");

            var fileName = "testimage.jpg";
            var fileStream = new MemoryStream(new byte[] { 0x20, 0x20 });
            var formFile = new FormFile(fileStream, 0, fileStream.Length, "ProductImages", fileName)
            {
                Headers = new HeaderDictionary(),
                ContentType = "image/jpeg"
            };

            // Add file to the content
            productImagesContent.Add(new StreamContent(formFile.OpenReadStream()), "ProductTestImages", fileName);

            // Convert FormFileCollection to a format that can be used in the request
            var formData = new MultipartFormDataContent
            {
                { new StringContent("Test Product"), "Name" },
                { new StringContent("10"), "Stock" },
                { new StringContent("100.00"), "Price" },
                { productImagesContent, "ProductImages" }
            };

            // Act
            var request = new HttpRequestMessage(HttpMethod.Post, "/api/products")
            {
                Content = formData
            };

            var response = await _client.SendAsync(request);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var content = await response.Content.ReadAsStringAsync();
            content.Should().Contain("Product created Successfully!");
        }

        [Fact]
        public async Task GetById_ShouldReturnOk_WithProduct()
        {
            // Step 1: Add a Product
            var productImagesContent = new MultipartFormDataContent();

            // Create a mock file
            var fileContent = new ByteArrayContent(new byte[] { 0x20, 0x20 }); // Mock file data
            fileContent.Headers.ContentType = MediaTypeHeaderValue.Parse("image/jpeg");

            var fileName = "testimage.jpg";
            var fileStream = new MemoryStream(new byte[] { 0x20, 0x20 });
            var formFile = new FormFile(fileStream, 0, fileStream.Length, "ProductImages", fileName)
            {
                Headers = new HeaderDictionary(),
                ContentType = "image/jpeg"
            };

            // Add file to the content
            productImagesContent.Add(new StreamContent(formFile.OpenReadStream()), "ProductTestImages", fileName);

            // Convert FormFileCollection to a format that can be used in the request
            var formData = new MultipartFormDataContent
            {
                { new StringContent("Test Product"), "Name" },
                { new StringContent("10"), "Stock" },
                { new StringContent("100.00"), "Price" },
                { productImagesContent, "ProductImages" }
            };

            // Act
            var request = new HttpRequestMessage(HttpMethod.Post, "/api/products")
            {
                Content = formData
            };

            var createProductResponse = await _client.SendAsync(request);
            createProductResponse.EnsureSuccessStatusCode();

            var createProductContent = await createProductResponse.Content.ReadAsStringAsync();

            // Step 2: Get All Products
            var getAllProductsResponse = await _client.GetAsync("/api/products");
            getAllProductsResponse.EnsureSuccessStatusCode();
            var allProductsContent = await getAllProductsResponse.Content.ReadAsStringAsync();
            var allProductsObject = JObject.Parse(allProductsContent);

            // Extract the products array
            var productsArray = allProductsObject["products"] as JArray;

            // Ensure there is at least one product and select the first one
            productsArray.Should().NotBeEmpty();
            var firstProduct = productsArray?.First;
            var productId = firstProduct?["id"]?.Value<string>(); // Adjust the path as needed

            // Step 3: Get Product by ID
            var getProductResponse = await _client.GetAsync($"/api/products/{productId}");
            getProductResponse.EnsureSuccessStatusCode();
            var productContent = await getProductResponse.Content.ReadAsStringAsync();

            // Assert
            productContent.Should().NotBeNullOrEmpty();
            // Optionally: Validate that the content matches the expected product details
        }

        [Fact]
        public async Task UpdateProduct_ShouldReturnOk_WithUpdatedProduct()
        {
            // Step 1: Create a Product
            var createProductRequest = new MultipartFormDataContent();
            createProductRequest.Add(new StringContent("Test Product"), "Name");
            createProductRequest.Add(new StringContent("10"), "Stock");
            createProductRequest.Add(new StringContent("100.00"), "Price");

            var createProductResponse = await _client.PostAsync("/api/products", createProductRequest);
            createProductResponse.EnsureSuccessStatusCode();
            var createProductContent = await createProductResponse.Content.ReadAsStringAsync();

            // Step 2: Get All Products
            var getAllProductsResponse = await _client.GetAsync("/api/products");
            getAllProductsResponse.EnsureSuccessStatusCode();
            var allProductsContent = await getAllProductsResponse.Content.ReadAsStringAsync();
            var allProductsObject = JObject.Parse(allProductsContent);

            // Extract the products array
            var productsArray = allProductsObject["products"] as JArray;

            // Ensure there is at least one product and select the first one
            productsArray.Should().NotBeNullOrEmpty();
            var firstProduct = productsArray?.First;
            var productId = firstProduct?["id"]?.ToString(); // Extract the product ID

            // Step 3: Update Product
            var updateProductRequest = new
            {
                
                Name = "Updated Product",
                Stock = 20,
                Price = 150.00f
            };

            var updateProductResponse = await _client.PutAsJsonAsync($"/api/products/{productId}", updateProductRequest);
            updateProductResponse.EnsureSuccessStatusCode();
            var updateProductContent = await updateProductResponse.Content.ReadAsStringAsync();

            // Assert
            updateProductContent.Should().NotBeNullOrEmpty();
            // Optionally: Validate that the content matches the updated product details

            // Step 4: Verify Update
            var getUpdatedProductResponse = await _client.GetAsync($"/api/products/{productId}");
            getUpdatedProductResponse.EnsureSuccessStatusCode();
            var updatedProductContent = await getUpdatedProductResponse.Content.ReadAsStringAsync();

            // Assert
            updatedProductContent.Should().Contain("Product updated successfully!");
        }

        [Fact]
        public async Task DeleteProduct_ShouldReturnOk_AndProductShouldBeDeleted()
        {
            // Step 1: Create a Product
            var createProductRequest = new MultipartFormDataContent();
            createProductRequest.Add(new StringContent("Test Product"), "Name");
            createProductRequest.Add(new StringContent("10"), "Stock");
            createProductRequest.Add(new StringContent("100.00"), "Price");

            var createProductResponse = await _client.PostAsync("/api/products", createProductRequest);
            createProductResponse.EnsureSuccessStatusCode();
            var createProductContent = await createProductResponse.Content.ReadAsStringAsync();

            // Step 2: Get All Products
            var getAllProductsResponse = await _client.GetAsync("/api/products");
            getAllProductsResponse.EnsureSuccessStatusCode();
            var allProductsContent = await getAllProductsResponse.Content.ReadAsStringAsync();
            var allProductsObject = JObject.Parse(allProductsContent);

            // Extract the products array
            var productsArray = allProductsObject["products"] as JArray;

            // Ensure there is at least one product and select the first one
            productsArray.Should().NotBeNullOrEmpty();
            var firstProduct = productsArray?.First;
            var productId = firstProduct?["id"]?.ToString(); // Extract the product ID

            // Step 3: Delete Product
            var deleteProductResponse = await _client.DeleteAsync($"/api/products/{productId}");
            deleteProductResponse.EnsureSuccessStatusCode();

            // Assert
            var deletedProductContent = await deleteProductResponse.Content.ReadAsStringAsync();
            deletedProductContent.Should().NotBeNullOrEmpty();
            deletedProductContent.Should().Contain("Product removed successfully!");

            // Step 4: Verify Deletion
            var verifyDeletionResponse = await _client.GetAsync($"/api/products/{productId}");
            verifyDeletionResponse.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }
    }
}

