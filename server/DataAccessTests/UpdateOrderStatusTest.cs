using DataAccess;
using PgCtx;
using SharedTestDependencies;
using Xunit;

namespace DataAccessTests;
    
public class UpdateOrderStatusTest
{
    private readonly PgCtxSetup<DMShopContext> _setup = new();
    
    [Fact]
    public void UpdateOrderStatus_ReturnsCorrectOrderAfterUpdate()
    {
        // Arrange:
        var testOrder = TestObjects.GetOrder();
        testOrder.Status = "processing";
        _setup.DbContextInstance.Orders.Add(testOrder);
        _setup.DbContextInstance.SaveChanges();

        // Act:
        var repository = new DMShopRepository(_setup.DbContextInstance);
        var updatedOrder = repository.UpdateOrderStatus(testOrder.Id, "cancelled");

        // Assert:
        Assert.NotNull(updatedOrder);
        Assert.Equal("cancelled", updatedOrder.Status);
    }
}
