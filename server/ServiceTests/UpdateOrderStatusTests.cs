using DataAccess.Interfaces;
using DataAccess.Models;
using Moq;
using Service;
using Service.Validators;

namespace ServiceTests;

public class UpdateOrderStatusTests
{
    private readonly Mock<IDmShopRepository> mockRepository;
    private readonly DmShopService service;
    private readonly UpdateOrderStatusValidator validator;

    public UpdateOrderStatusTests()
    {
        mockRepository = new Mock<IDmShopRepository>();
        validator = new UpdateOrderStatusValidator();
        service = new DmShopService(mockRepository.Object, validator);
    }

    [Fact]
    public void UpdateOrderStatus_Should_UpdateStatus_When_Status_Is_Valid()
    {
        // Arrange
        var orderId = 1;
        var newStatus = "delivered";
        var order = new Order { Id = orderId, Status = "processing" };
        
        mockRepository.Setup(r => r.UpdateOrderStatus(orderId, newStatus))
            .Returns(new Order { Id = orderId, Status = newStatus });

        // Act
        var updatedOrder = service.UpdateOrderStatus(orderId, newStatus);

        // Assert
        Assert.Equal(newStatus, updatedOrder.Status);
        mockRepository.Verify(r => r.UpdateOrderStatus(orderId, newStatus));
    }

    [Fact]
    public void UpdateOrderStatus_Should_ThrowsException_When_Status_Is_Invalid()
    {
        // Arrange
        var orderId = 1;
        var invalidStatus = "unknown_status";

        // Act & Assert
        Assert.Throws<ServiceException>(() => service.UpdateOrderStatus(orderId, invalidStatus));
    }
}