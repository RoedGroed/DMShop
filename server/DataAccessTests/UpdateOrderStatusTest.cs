using DataAccess;
using PgCtx;
using SharedTestDependencies;
using Xunit;

public class UpdateOrderStatusTest
{
    private readonly PgCtxSetup<DMShopContext> setup = new();
    
    [Fact]
    public void UpdateOrderStatus_ReturnsCorrectOrderAfterUpdate()
    {
        // Arrange:
        var testOrder = TestObjects.GetOrder();
        testOrder.Status = "processing";
        setup.DbContextInstance.Orders.Add(testOrder);
        setup.DbContextInstance.SaveChanges();

        // Act:
        var repository = new DMShopRepository(setup.DbContextInstance);
        var updatedOrder = repository.UpdateOrderStatus(testOrder.Id, "cancelled");

        // Assert:
        Assert.NotNull(updatedOrder);
        Assert.Equal("cancelled", updatedOrder.Status);
    }
}
