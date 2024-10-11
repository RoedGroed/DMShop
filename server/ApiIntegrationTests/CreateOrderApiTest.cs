using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using API;
using DataAccess;
using Microsoft.AspNetCore.Mvc.Testing;
using PgCtx;
using Service.TransferModels.Requests;
using Service.TransferModels.Responses;
using Xunit;

namespace ApiIntegrationTests;

public class CreateOrderApiTest : WebApplicationFactory<Program>
{
    private readonly PgCtxSetup<DMShopContext> _pgCtxSetup = new();
    private readonly JsonSerializerOptions _jsonOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

    public CreateOrderApiTest()
    {
        Environment.SetEnvironmentVariable("DbConnectionString", _pgCtxSetup._postgres.GetConnectionString());
    }

    [Fact]
    public async Task CreateOrder_ReturnsCreatedOrder()
    {
        // Arrange
        var createOrderDto = new CreateOrderDTO
        {
            CustomerId = 1,
            Items = new List<OrderEntryRequestDTO>
            {
                new OrderEntryRequestDTO { ProductId = 1, Quantity = 2 },
                new OrderEntryRequestDTO { ProductId = 2, Quantity = 3 }
            },
            OrderEntries = new List<OrderEntryRequestDTO> // Ensure this is included
            {
                new OrderEntryRequestDTO { ProductId = 1, Quantity = 2 },
                new OrderEntryRequestDTO { ProductId = 2, Quantity = 3 }
            },
            DeliveryDate = DateTime.UtcNow.AddDays(5),
            OrderDate = DateTime.UtcNow,
            Status = "Pending", // Ensure this matches API expectations
            TotalAmount = 68.25 // Assuming a total amount
        };


        // Act
        var response = await CreateClient().PostAsJsonAsync("api/Order/create", createOrderDto);
        var jsonResponse = await response.Content.ReadAsStringAsync();
        var result = JsonSerializer.Deserialize<OrderDto>(jsonResponse, _jsonOptions);

        // Assert
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        Assert.NotNull(result);
        Assert.True(result.Id > 0);
        Assert.Equal(createOrderDto.CustomerId, result.CustomerId);
        Assert.Equal(createOrderDto.Status, result.Status);
        Assert.Equal(createOrderDto.TotalAmount, result.TotalAmount);

        // Database check
        using (var scope = _pgCtxSetup.DbContextInstance)
        {
            var createdOrder = await scope.Orders.FindAsync(result.Id);
            Assert.NotNull(createdOrder);
            Assert.Equal(createOrderDto.CustomerId, createdOrder.CustomerId);
            Assert.Equal(createOrderDto.Status, createdOrder.Status);
            Assert.Equal(createOrderDto.TotalAmount, createdOrder.TotalAmount);
            Assert.Equal(createOrderDto.Items.Count, createdOrder.OrderEntries.Count);
        }
    }
}
