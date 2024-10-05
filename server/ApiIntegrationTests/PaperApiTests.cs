/*
using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using API;
using DataAccess;
using DataAccess.Models;
using PgCtx;
using Service.TransferModels.Responses;
using SharedTestDependencies;
using Xunit;
using FluentAssertions;

namespace ApiIntegrationTests
{
    public class PapersApiTests
    {
        private readonly PgCtxSetup<DMShopContext> _pgCtxSetup;
        private readonly JsonSerializerOptions _jsonOptions;

        public PapersApiTests()
        {
            _pgCtxSetup = new PgCtxSetup<DMShopContext>();
            Environment.SetEnvironmentVariable("DbConnectionString", _pgCtxSetup._postgres.GetConnectionString());
            _jsonOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        }

        [Fact]
        public async Task GetAllPapers_WithProperties_ReturnsOkAndData()
        {
            // Arrange
            using (var context = _pgCtxSetup.DbContextInstance)
            {
                var papers = TestObjects.GetPapers(10, 5);
                context.Papers.AddRange(papers);
                await context.SaveChangesAsync();
            }

            // Act
            var client = new HttpClient { BaseAddress = new Uri("http://localhost:5000") }; // Use your actual base URL
            var response = await client.GetAsync("/api/Product/with-properties");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var returnedPapers = await response.Content.ReadFromJsonAsync<List<ProductDto>>();
            returnedPapers.Should().NotBeNull();
            returnedPapers.Should().HaveCountGreaterThan(0);
            returnedPapers.First().Properties.Should().NotBeNullOrEmpty();
            returnedPapers.First().Properties.Should().HaveCount(2);
        }

        [Fact]
        public async Task CreatePaper_ValidData_ReturnsOkAndCreatedPaper()
        {
            // Arrange
            var newPaper = TestObjects.GetPapers(1, 2).First();
            var newPaperDto = new ProductDto
            {
                Name = newPaper.Name,
                Price = newPaper.Price,
                Stock = newPaper.Stock,
                Discontinued = newPaper.Discontinued,
                Properties = new List<PropertyDto>
                {
                    new PropertyDto { Id = 1, PropertyName = "Property 1" }, 
                    new PropertyDto { Id = 2, PropertyName = "Property 2" }
                }
            };

            var client = new HttpClient { BaseAddress = new Uri("http://localhost:5000") }; // Use your actual base URL

            // Act
            var response = await client.PostAsJsonAsync("/api/Product", newPaperDto);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.Created);
            var createdPaper = await response.Content.ReadFromJsonAsync<ProductDto>();
            createdPaper.Should().NotBeNull();
            createdPaper.Name.Should().Be(newPaperDto.Name);
            createdPaper.Price.Should().Be(newPaperDto.Price);
            createdPaper.Stock.Should().Be(newPaperDto.Stock);

            // Check that properties were created correctly
            createdPaper.Properties.Should().NotBeNull();
            createdPaper.Properties.Should().HaveCount(newPaperDto.Properties.Count);
        }

        [Fact]
        public async Task UpdatePaper_ValidData_ReturnsOkAndUpdatedPaper()
        {
            // Arrange
            var initialPaper = TestObjects.GetPapers(1, 2).First();
            using (var context = _pgCtxSetup.DbContextInstance)
            {
                context.Papers.Add(initialPaper);
                await context.SaveChangesAsync();
            }

            var updatedPaperDto = new ProductDto
            {
                Id = initialPaper.Id,
                Name = "Updated Paper Name",
                Price = 19.99,
                Stock = 150,
                Discontinued = true,
                Properties = initialPaper.Properties.Select(prop => new PropertyDto
                {
                    Id = prop.Id,
                    PropertyName = prop.PropertyName
                }).ToList()
            };

            var client = new HttpClient { BaseAddress = new Uri("http://localhost:5000") }; // Use your actual base URL

            // Act
            var updateResponse = await client.PutAsJsonAsync($"/api/Product/{updatedPaperDto.Id}", updatedPaperDto);

            // Assert
            updateResponse.StatusCode.Should().Be(HttpStatusCode.OK);
            var returnedPaper = await updateResponse.Content.ReadFromJsonAsync<ProductDto>();
            returnedPaper.Should().NotBeNull();
            returnedPaper.Name.Should().Be(updatedPaperDto.Name);
            returnedPaper.Price.Should().Be(updatedPaperDto.Price);
            returnedPaper.Stock.Should().Be(updatedPaperDto.Stock);
            returnedPaper.Properties.Should().HaveCount(updatedPaperDto.Properties.Count);
        }
    }
}
*/
