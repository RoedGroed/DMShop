using DataAccess;
using DataAccess.Models;
using PgCtx;
using SharedTestDependencies;
using Xunit;

public class GetOrdersForListTest
{
    private readonly PgCtxSetup<DMShopContext> _setup = new();

    [Fact]
    public void GetOrdersForList_ReturnsCorrectOrders()
    {
        // Arrange
        var orders = new List<Order>
        {
            TestObjects.GetOrder(),
            TestObjects.GetOrder(),
            TestObjects.GetOrder()
        };
        _setup.DbContextInstance.Orders.AddRange(orders);
        _setup.DbContextInstance.SaveChanges();

        // Act
        var result = new DMShopRepository(_setup.DbContextInstance).GetOrdersForList(2, 0);

        // Assert
        Assert.Equal(2, result.Count);
        Assert.All(result, order =>
        {
            Assert.NotNull(order.Customer);
        });
    }

    [Fact]
    public void GetOrdersForList_WithPagination_ReturnsCorrectOrder()
    {
        var orders = new List<Order>
        {
            TestObjects.GetOrder(),
            TestObjects.GetOrder(),
            TestObjects.GetOrder()
        };
        _setup.DbContextInstance.Orders.AddRange(orders);
        _setup.DbContextInstance.SaveChanges();

        // Act:
        var result = new DMShopRepository(_setup.DbContextInstance).GetOrdersForList(2, 2);

        Assert.Single(result);

        var fetchedOrder = result.First();
        var expectedOrder = orders.Skip(2).First();
        Assert.Equal(expectedOrder.Customer.Name, fetchedOrder.Customer.Name);
        Assert.Equal(expectedOrder.OrderDate.ToString("yyyy-MM-dd h:mm:ss tt zz"), fetchedOrder.OrderDate.ToString("yyyy-MM-dd h:mm:ss tt zz"));
        Assert.Equal(expectedOrder.DeliveryDate, fetchedOrder.DeliveryDate);
        Assert.Equal(expectedOrder.Status, fetchedOrder.Status);
        Assert.Equal(expectedOrder.TotalAmount, fetchedOrder.TotalAmount);
    }
}
