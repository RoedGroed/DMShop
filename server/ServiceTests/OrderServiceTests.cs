using DataAccess;
using DataAccess.Models;
using PgCtx;
using Service;
using SharedTestDependencies;
using Xunit;



namespace ServiceTests
{
    public class OrderServiceTests
    {
        private readonly PgCtxSetup<DMShopContext> _pgCtxSetup = new();
        private readonly DMShopService _orderService;

        public OrderServiceTests()
        {
            var repository = new DMShopRepository(_pgCtxSetup.DbContextInstance);
            _orderService = new DMShopService(repository);
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

            _pgCtxSetup.DbContextInstance.Orders.AddRange(orders);
            _pgCtxSetup.DbContextInstance.SaveChanges();

            // Act
            var result = _orderService.GetOrdersForList(2, 0);

            // Assert
            Assert.Equal(2, result.Count);
            Assert.Equal(orders[0].Customer.Name, result[0].CustomerName);
            Assert.Equal(orders[1].Customer.Name, result[1].CustomerName);
        }
    }
}