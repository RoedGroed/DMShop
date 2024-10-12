using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using API;
using DataAccess;
using DataAccess.Models;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using SharedTestDependencies;
using PgCtx;
using Service.TransferModels.Requests;
using Service.TransferModels.Requests.Orders;
using Service.TransferModels.Responses;
using Service.TransferModels.Responses.Orders;

namespace ApiIntegrationTests
{
    public class OrdersApiTests : WebApplicationFactory<Program>
    {
        private readonly PgCtxSetup<DmShopContext> setup = new();
        private readonly JsonSerializerOptions options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

        public OrdersApiTests()
        {
            Environment.SetEnvironmentVariable("DbConnectionString", setup._postgres.GetConnectionString());
        }

        [Fact]
        public async Task GetOrdersForList_ReturnsCorrectOrders()
        {
            // Arrange
            var orders = new List<Order> { TestObjects.GetOrder(), TestObjects.GetOrder() };
            setup.DbContextInstance.Orders.AddRange(orders);
            await setup.DbContextInstance.SaveChangesAsync();

            // Act
            var response = await CreateClient().GetAsync("/api/order?limit=2&startAt=0");
            var jsonResponse = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<List<OrderListDto>>(jsonResponse, options);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
            Assert.Equal(orders[0].Id, result[0].Id);
            Assert.Equal(orders[0].Customer.Name, result[0].CustomerName);
            Assert.Equal(orders[1].Customer.Name, result[1].CustomerName);
        }

        [Fact]
        public async Task UpdateOrderStatus_ReturnsUpdatedStatus()
        {
            // Arrange
            var testOrder = TestObjects.GetOrder();
            testOrder.Status = "pending";
            setup.DbContextInstance.Orders.Add(testOrder);
            await setup.DbContextInstance.SaveChangesAsync();

            var updateStatusDto = new UpdateOrderStatusDto()
            {
                NewStatus = "processing"
            };
            
            // Act
            var response = await CreateClient().PutAsJsonAsync($"/api/Order/{testOrder.Id}/status", updateStatusDto);
                
            // Assert
            var updatedOrder = await setup.DbContextInstance.Orders.AsNoTracking().FirstOrDefaultAsync(o => o.Id == testOrder.Id);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.NotNull(updatedOrder);
            Assert.Equal("processing", updatedOrder.Status);
        }
    }
}