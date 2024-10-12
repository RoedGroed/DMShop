using DataAccess;
using DataAccess.Interfaces;
using DataAccess.Models;
using FluentValidation;
using Service.TransferModels.Requests.Orders;
using Service.TransferModels.Responses.Orders;
using Service.TransferModels.Responses.Products;
using Service.Validators;

namespace Service;

public interface IDmShopService
{

    OrderDto CreateOrder(CreateOrderDto createOrderDto);
    public List<ProductDto> GetAllPapers();
    public List<ProductDto> GetAllPapersWithProperties();
    public ProductDto CreatePaper(ProductDto productDto);
    ProductDto DeletePaper(int id, ProductDto productDto);
    ProductDto UpdatePaper(int id, ProductDto productDto, List<int> propertyIds);
    ProductDto GetPaperById(int id);
    List<PropertyDto> GetAllProperties();
    PropertyDto CreateProperty(PropertyDto propertyDto);
    PropertyDto DeleteProperty(int propertyId);
    PropertyDto UpdateProperty(PropertyDto propertyDto);
    public List<OrderListDto> GetOrdersForList(int limit, int startAt);
    public OrderDetailsDto GetOrderDetailsById(int id);
    public List<OrderListDto> GetRandomCustomerOrderHistory();
    public Order UpdateOrderStatus (int orderId, string newStatus);
}


public class DmShopService(
    IDmShopRepository dmShopRepository,
    IValidator<UpdateOrderStatusDto> updateOrderStatusValidator
    ) :IDmShopService
{
    public List<ProductDto> GetAllPapers()
    {
        var papers = dmShopRepository.GetAllPapers();
        return papers.Select(p => new ProductDto()
        {
            Id = p.Id,
            Name = p.Name,
            Price = p.Price,
            Stock = p.Stock,
            Discontinued = p.Discontinued
        }).ToList();
    }

    public List<ProductDto> GetAllPapersWithProperties()
    {
        var papers = dmShopRepository.GetAllPapersWithProperties();

        // Map the papers to ProductDto, including the properties
        return papers.Select(p => new ProductDto
        {
            Id = p.Id,
            Name = p.Name,
            Price = p.Price,
            Stock = p.Stock,
            Discontinued = p.Discontinued,
            Properties = p.Properties.Select(prop => new PropertyDto
            {
                Id = prop.Id,
                PropertyName = prop.PropertyName
            }).ToList()
        }).ToList();
    }

    public ProductDto CreatePaper(ProductDto productDto)
    {
        // Validate the incoming product data
        var validator = new CreatePaperValidator();
        validator.ValidateAndThrow(productDto);

        // Map the incoming DTO to the Paper entity
        var paper = new Paper
        {
            Name = productDto.Name,
            Price = productDto.Price,
            Stock = productDto.Stock,
            Discontinued = productDto.Discontinued = false
        };

        // Extract property IDs from the productDto, if any
        var propertyIds = productDto.Properties.Select(p => p.Id).ToList();

        // Call repository to create the paper and associate properties at the same time
        var createdPaper = dmShopRepository.CreatePaper(paper, propertyIds);

        return ProductDto.FromEntity(createdPaper);
    }


    public ProductDto DeletePaper(int id, ProductDto productDto)
    {
        // Extract property IDs from the productDto
        var propertyIds = productDto.Properties.Select(p => p.Id).ToList();

        // Call the repository to delete the paper along with specified properties
        var deletedPaper = dmShopRepository.DeletePaper(id, propertyIds);
        return ProductDto.FromEntity(deletedPaper);

    }

    public ProductDto UpdatePaper(int id, ProductDto productDto, List<int> propertyIds)
    {

        var validator = new CreatePaperValidator();
        validator.ValidateAndThrow(productDto);

        var paper = new Paper
        {
            Id = id, 
            Name = productDto.Name,
            Price = productDto.Price,
            Stock = productDto.Stock,
            Discontinued = productDto.Discontinued
        };

        // Call the repository to update the paper and its properties
        var updatedPaper = dmShopRepository.UpdatePaper(paper, propertyIds);

        return ProductDto.FromEntity(updatedPaper);
    }

    public ProductDto GetPaperById(int id)
    {
        var paper = dmShopRepository.GetPaperById(id);
        return ProductDto.FromEntity(paper);
    }

    public List<PropertyDto> GetAllProperties()
    {
        var properties = dmShopRepository.GetAllProperties();
        return properties.Select(p => new PropertyDto()
        {
            Id = p.Id,
            PropertyName = p.PropertyName

        }).ToList();
    }
    
    public PropertyDto CreateProperty(PropertyDto propertyDto)
    {
        var validator = new PropertyValidator();
        validator.ValidateAndThrow(propertyDto);
        
        var property = new Property
        {
            PropertyName = propertyDto.PropertyName
        };
        var createdProperty = dmShopRepository.CreateProperty(property);
        return new PropertyDto { Id = createdProperty.Id, PropertyName = createdProperty.PropertyName };
    }
    
    public PropertyDto DeleteProperty(int propertyId)
    {
        dmShopRepository.DeleteProperty(propertyId);
        return null!; 
    }

    public PropertyDto UpdateProperty(PropertyDto propertyDto)
    {
        
        var validator = new PropertyValidator();
        validator.ValidateAndThrow(propertyDto);

        var property = new Property
        {
            Id = propertyDto.Id,
            PropertyName = propertyDto.PropertyName
        };

        var updatedProperty = dmShopRepository.UpdateProperty(property);
        return new PropertyDto { Id = updatedProperty.Id, PropertyName = updatedProperty.PropertyName };
    }

    public List<OrderListDto> GetOrdersForList(int limit, int startAt)
    {
        try
        {
            var orders = dmShopRepository.GetOrdersForList(limit, startAt);
            return orders.Select(order => OrderListDto.FromEntity(order)).ToList();
        }
        catch (DataAccessException)
        {
            throw new ServiceException("Failed to retrieve orders from the service layer");
        }    
    }

    public OrderDto CreateOrder(CreateOrderDto createOrderDto)
    {
        double totalAmount = 0;
        
        var paperIds = createOrderDto.OrderEntries.Select(o => o.ProductId).ToList();
        var papers = dmShopRepository.GetPaperByIds(paperIds);

        foreach (var entry in createOrderDto.OrderEntries)
        {
            var paper = papers.FirstOrDefault(p => p.Id == entry.ProductId);
            if (paper != null)
            {
                totalAmount += paper.Price * entry.Quantity;
            }
        }
        
        var order = new Order
        {
            OrderDate = createOrderDto.OrderDate,
            DeliveryDate = DateOnly.FromDateTime(createOrderDto.DeliveryDate),
            Status = createOrderDto.Status,
            TotalAmount = totalAmount,
            CustomerId = createOrderDto.CustomerId
        };
        var orderEntries = createOrderDto.OrderEntries.Select(entry => new OrderEntry
        {
            ProductId = entry.ProductId,
            Quantity = entry.Quantity,
            Order = order
        }).ToList();
        var createdOrder = dmShopRepository.CreateOrder(order, orderEntries);
        return OrderDto.FromEntity(createdOrder);
    }
    

    public OrderDetailsDto GetOrderDetailsById(int orderId)
    {
        try
        {
            var order = dmShopRepository.GetOrderDetailsById(orderId);
            return OrderDetailsDto.FromEntity(order);
        }
        catch (DataAccessException ex)
        {
            throw new ServiceException(ex.Message);
        }
    }

    public List<OrderListDto> GetRandomCustomerOrderHistory()
    {
        try
        {
            var customer = dmShopRepository.GetRandomCustomer();
            if (customer == null) throw new ServiceException("No customers found.");

            var orders = dmShopRepository.GetOrdersForCustomer(customer.Id);
            if (orders == null || !orders.Any()) throw new ServiceException("No orders found for the customer.");
            
            return orders.Select(order => OrderListDto.FromEntity(order)).ToList();
        }
        catch (DataAccessException ex)
        {
            throw new ServiceException(ex.Message);
        }
    }


    public Order UpdateOrderStatus(int orderId, string newStatus)
    {
        try
        {
            var updateStatusDto = new UpdateOrderStatusDto { NewStatus = newStatus };
            updateOrderStatusValidator.ValidateAndThrow(updateStatusDto);
            
            var updatedOrder = dmShopRepository.UpdateOrderStatus(orderId, newStatus);
            
            return updatedOrder;
        }
        catch (ValidationException)
        {
            throw new ServiceException("Invalid status update request");
        }
        catch (DataAccessException ex)
        {
            throw new ServiceException(ex.Message);
        }    
    }
}