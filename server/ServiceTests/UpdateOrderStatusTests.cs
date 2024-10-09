using DataAccess.Interfaces;
using DataAccess.Models;
using FluentValidation;
using Moq;
using Service;
using Service.Validators;

namespace ServiceTests;

public class UpdateOrderStatusTests
{
    private readonly Mock<IDMShopRepository> mockRepository;
    private readonly DMShopService service;
    private readonly UpdateOrderStatusValidator validator;

    public UpdateOrderStatusTests()
    {
        mockRepository = new Mock<IDMShopRepository>();
        validator = new UpdateOrderStatusValidator();
        service = new DMShopService(mockRepository.Object, validator);
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
    public void UpdateOrderStatus_Should_ThrowValidationException_When_Status_Is_Invalid()
    {
        // Arrange
        var orderId = 1;
        var invalidStatus = "unknown_status";

        // Act & Assert
        Assert.Throws<ValidationException>(() => service.UpdateOrderStatus(orderId, invalidStatus));
    }
}