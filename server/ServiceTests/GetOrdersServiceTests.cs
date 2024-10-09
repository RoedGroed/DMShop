using DataAccess;
using DataAccess.Models;
using PgCtx;
using Service;
using SharedTestDependencies;
using Xunit;

namespace ServiceTests;

    public class GetOrdersServiceTests
    {
        private readonly PgCtxSetup<DMShopContext> setup = new();
        private readonly DMShopService orderService;

        public GetOrdersServiceTests()
        {
            var repository = new DMShopRepository(setup.DbContextInstance);
            orderService = new DMShopService(repository, null);
        }

        [Fact]
        public void GetOrdersForList_ReturnsCorrectOrders()
        {
            // Arrange
            var orders = new List<Order>
            {
                TestObjects.GetOrder(),
                TestObjects.GetOrder()
            };

            setup.DbContextInstance.Orders.AddRange(orders);
            setup.DbContextInstance.SaveChanges();

            // Act
            var result = orderService.GetOrdersForList(2, 0);

            // Assert
            Assert.Equal(2, result.Count);
            Assert.Equal(orders[0].Customer.Name, result[0].CustomerName);
            Assert.Equal(orders[1].Customer.Name, result[1].CustomerName);
        }
    }
