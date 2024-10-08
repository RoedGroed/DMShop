using System.Net;
using System.Text.Json;
using API;
using DataAccess;
using DataAccess.Models;
using Microsoft.AspNetCore.Mvc.Testing;
using SharedTestDependencies;
using PgCtx;
using Service.TransferModels.Responses;

namespace ApiIntegrationTests
{
    public class OrdersApiTests : WebApplicationFactory<Program>
    {
        private readonly PgCtxSetup<DMShopContext> _pgCtxSetup = new();
        private readonly JsonSerializerOptions _jsonOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

        public OrdersApiTests()
        {
            Environment.SetEnvironmentVariable("DbConnectionString", _pgCtxSetup._postgres.GetConnectionString());
        }

        [Fact]
        public async Task GetOrdersForList_ReturnsCorrectOrders()
        {
            // Arrange
            var orders = new List<Order> { TestObjects.GetOrder(), TestObjects.GetOrder() };
            _pgCtxSetup.DbContextInstance.Orders.AddRange(orders);
            await _pgCtxSetup.DbContextInstance.SaveChangesAsync();

            // Act
            var response = await CreateClient().GetAsync("/api/order?limit=2&startAt=0");
            var jsonResponse = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<List<OrderListDto>>(jsonResponse, _jsonOptions);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
            Assert.Equal(orders[0].Id, result[0].Id);
            Assert.Equal(orders[0].Customer.Name, result[0].CustomerName);
            Assert.Equal(orders[1].Customer.Name, result[1].CustomerName);
        }
    }
}