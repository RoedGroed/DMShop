using DataAccess.Interfaces;
using DataAccess.Models;
using FluentValidation;
using Moq;
using Service;
using Service.TransferModels.Requests;
using Service.TransferModels.Requests.Orders;
using Service.TransferModels.Responses;
using Xunit;

namespace ServiceTests;

public class CreateOrderServiceTest
{
    private readonly Mock<IDmShopRepository> mockRepository;
    private readonly DmShopService service;

    public CreateOrderServiceTest()
    {
        mockRepository = new Mock<IDmShopRepository>();
        service = new DmShopService(mockRepository.Object, null);
    }

    [Fact]
    public void CreateOrder_Should_Return_CreatedOrder_When_ValidData_Provided()
    {
        // Arrange
        var createOrderDto = new CreateOrderDto
        {
            CustomerId = 1,
            OrderDate = DateTime.UtcNow,
            DeliveryDate = DateTime.UtcNow.AddDays(1),
            Status = "pending",
            OrderEntries = new List<OrderEntryRequestDto>
            {
                new OrderEntryRequestDto { ProductId = 1, Quantity = 2 }
            }
        };

        var expectedOrder = new Order
        {
            Id = 1,
            CustomerId = createOrderDto.CustomerId,
            OrderDate = createOrderDto.OrderDate,
            DeliveryDate = DateOnly.FromDateTime(createOrderDto.DeliveryDate),
            Status = createOrderDto.Status,
            TotalAmount = 21.0
        };

        mockRepository.Setup(repo => repo.GetPaperByIds(It.IsAny<List<int>>()))
            .Returns(new List<Paper>
            {
                new Paper { Id = 1, Price = 10.5 }
            });

        mockRepository.Setup(repo => repo.CreateOrder(It.IsAny<Order>(), It.IsAny<List<OrderEntry>>()))
            .Returns(expectedOrder);

        // Act
        var createdOrder = service.CreateOrder(createOrderDto);

        // Assert
        Assert.NotNull(createdOrder);
        Assert.Equal(expectedOrder.CustomerId, createdOrder.CustomerId);
        Assert.Equal(expectedOrder.Status, createdOrder.Status);
        Assert.Equal(expectedOrder.TotalAmount, createdOrder.TotalAmount);
        mockRepository.Verify(repo => repo.CreateOrder(It.IsAny<Order>(), It.IsAny<List<OrderEntry>>()), Times.Once);
    }
    
}
