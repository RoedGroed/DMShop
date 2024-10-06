using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using API;
using DataAccess;
using DataAccess.Models;
using Microsoft.AspNetCore.Mvc.Testing;
using SharedTestDependencies;
using PgCtx;
using Service.TransferModels.Responses;
using Xunit;


namespace ApiIntegrationTests;

public class PaperApiTests : WebApplicationFactory<Program>
{
    private readonly PgCtxSetup<DMShopContext> _pgCtxSetup = new();
    private readonly JsonSerializerOptions _jsonOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

    public PaperApiTests()
    {
        Environment.SetEnvironmentVariable("DbConnectionString", _pgCtxSetup._postgres.GetConnectionString());
    }

    [Fact]
    public async Task GetPapersList_ReturnsCorrectPapers()
    {
        //Arrange
        var papers = TestObjects.GetPapers(10, 5);
        _pgCtxSetup.DbContextInstance.Papers.AddRange(papers);
        await _pgCtxSetup.DbContextInstance.SaveChangesAsync();
        
        
        // Act
        var response = await CreateClient().GetAsync("api/Product/with-properties");
        var jsonResponse = await response.Content.ReadAsStringAsync();
        var result = JsonSerializer.Deserialize<List<ProductDto>>(jsonResponse, _jsonOptions);
        
        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.NotNull(result);
        Assert.NotEmpty(result); 

        // Assert properties for the first paper
        Assert.Equal(papers[0].Id, result[0].Id);
        Assert.Equal(papers[0].Name, result[0].Name);
        Assert.Equal(papers[0].Discontinued, result[0].Discontinued);
        Assert.Equal(papers[0].Stock, result[0].Stock);
        Assert.Equal(papers[0].Price, result[0].Price);
        Assert.Equal(papers[0].Properties.Count, result[0].Properties.Count);

        // Assert properties for the second paper
        Assert.Equal(papers[1].Id, result[1].Id);
        Assert.Equal(papers[1].Name, result[1].Name);
        Assert.Equal(papers[1].Discontinued, result[1].Discontinued);
        Assert.Equal(papers[1].Stock, result[1].Stock);
        Assert.Equal(papers[1].Price, result[1].Price);
        Assert.Equal(papers[1].Properties.Count, result[1].Properties.Count);
    }
    
    [Fact]
    public async Task CreatePaper_ReturnsCreatedPaper()
    {
        // Arrange
        var newPaperDto = new ProductDto
        {
            Name = "New Paper",
            Discontinued = false,
            Stock = 100,
            Price = 19.99,
            Properties = new List<PropertyDto>
            {
                new PropertyDto { Id = 1, PropertyName = "Property 1" },
                new PropertyDto { Id = 2, PropertyName = "Property 2" }
            }
        };

        // Act
        var response = await CreateClient().PostAsJsonAsync("api/Product", newPaperDto);
        var jsonResponse = await response.Content.ReadAsStringAsync();
        var result = JsonSerializer.Deserialize<ProductDto>(jsonResponse, _jsonOptions);
    
        // Assert
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        Assert.NotNull(result);
        Assert.True(result.Id > 0); // Ensure that the Id is set and valid
        Assert.Equal(newPaperDto.Name, result.Name);
        Assert.Equal(newPaperDto.Discontinued, result.Discontinued);
        Assert.Equal(newPaperDto.Stock, result.Stock);
        Assert.Equal(newPaperDto.Price, result.Price);
        Assert.NotEmpty(newPaperDto.Properties);
    
        /*//  check if the paper was actually created in the database
        using (var scope = _pgCtxSetup.DbContextInstance)
        {
            var createdPaper = await scope.Papers.FindAsync(result.Id);
            Assert.NotNull(createdPaper);
            Assert.Equal(newPaperDto.Name, createdPaper.Name);
            Assert.Equal(newPaperDto.Discontinued, createdPaper.Discontinued);
            Assert.Equal(newPaperDto.Stock, createdPaper.Stock);
            Assert.Equal(newPaperDto.Price, createdPaper.Price);
            Assert.NotEmpty(newPaperDto.Properties);
        }*/
    }
    
    
    [Fact]
    public async Task UpdatePaper_ReturnsUpdatedPaper()
    {
        // Arrange
        // Create a paper to update
        var initialPaperDto = new ProductDto
        {
            Name = "Initial Paper",
            Discontinued = false,
            Stock = 100,
            Price = 19.99,
            Properties = new List<PropertyDto>
            {
                new PropertyDto { Id = 1, PropertyName = "Property 1" },
                new PropertyDto { Id = 2, PropertyName = "Property 2" }
            }
        };

        // Create the initial paper
        var createResponse = await CreateClient().PostAsJsonAsync("api/Product", initialPaperDto);
        var createdPaper = await createResponse.Content.ReadFromJsonAsync<ProductDto>();

        // Prepare the updated paper DTO
        var updatedPaperDto = new ProductDto
        {
            Id = createdPaper.Id, // Set the Id to the created paper Id
            Name = "Updated Paper",
            Discontinued = true,
            Stock = 50,
            Price = 29.99,
            Properties = new List<PropertyDto>
            {
                new PropertyDto { Id = 1, PropertyName = "Updated Property 1" }, // Updating property
                new PropertyDto { Id = 2, PropertyName = "Property 2" } // Keeping existing property
            }
        };

        // Act
        var updateResponse = await CreateClient().PutAsJsonAsync($"api/Product/{updatedPaperDto.Id}", updatedPaperDto);

        // Assert
        Assert.Equal(HttpStatusCode.OK, updateResponse.StatusCode);
        var updatedPaper = await updateResponse.Content.ReadFromJsonAsync<ProductDto>();
        Assert.NotNull(updatedPaper);
        Assert.Equal(updatedPaperDto.Name, updatedPaper.Name);
        Assert.Equal(updatedPaperDto.Price, updatedPaper.Price);
        Assert.Equal(updatedPaperDto.Stock, updatedPaper.Stock);
        Assert.Equal(updatedPaperDto.Discontinued, updatedPaper.Discontinued);
        Assert.NotNull(updatedPaper.Properties);
        
        using (var scope = _pgCtxSetup.DbContextInstance)
        {
            var paperInDb = await scope.Papers.FindAsync(updatedPaper.Id);
            Assert.NotNull(paperInDb);
            Assert.Equal(updatedPaperDto.Name, paperInDb.Name);
            Assert.Equal(updatedPaperDto.Price, paperInDb.Price);
            Assert.Equal(updatedPaperDto.Stock, paperInDb.Stock);
            Assert.Equal(updatedPaperDto.Discontinued, paperInDb.Discontinued);
        }
}

}
