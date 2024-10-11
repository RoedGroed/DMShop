using DataAccess;
using DataAccess.Interfaces;
using DataAccess.Models;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Service.TransferModels.Requests;
using Service.TransferModels.Responses;
using Service.Validators;

namespace Service;

public interface IDMShopService
{

    OrderDto CreateOrder(CreateOrderDTO createOrderDto);
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


public class DMShopService(
    IDMShopRepository DMShopRepository,
    IValidator<UpdateOrderStatusDTO> updateOrderStatusValidator
    ) :IDMShopService
{
    public List<ProductDto> GetAllPapers()
    {
        var papers = DMShopRepository.GetAllPapers();
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
        var papers = DMShopRepository.GetAllPapersWithProperties();

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
        var propertyIds = productDto.Properties?.Select(p => p.Id).ToList() ?? new List<int>();

        // Call repository to create the paper and associate properties at the same time
        var createdPaper = DMShopRepository.CreatePaper(paper, propertyIds);

        return ProductDto.FromEntity(createdPaper);
    }


    public ProductDto DeletePaper(int id, ProductDto productDto)
    {
        // Extract property IDs from the productDto
        var propertyIds = productDto.Properties?.Select(p => p.Id).ToList() ?? new List<int>();

        // Call the repository to delete the paper along with specified properties
        var deletedPaper = DMShopRepository.DeletePaper(id, propertyIds);
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
        var updatedPaper = DMShopRepository.UpdatePaper(paper, propertyIds);

        return ProductDto.FromEntity(updatedPaper);
    }

    public ProductDto GetPaperById(int id)
    {
        var paper = DMShopRepository.GetPaperById(id);
        return paper != null ? ProductDto.FromEntity(paper) : null;
    }

    public List<PropertyDto> GetAllProperties()
    {
        var properties = DMShopRepository.GetAllProperties();
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
        var createdProperty = DMShopRepository.CreateProperty(property);
        return new PropertyDto { Id = createdProperty.Id, PropertyName = createdProperty.PropertyName };
    }
    
    public PropertyDto DeleteProperty(int propertyId)
    {
        DMShopRepository.DeleteProperty(propertyId);
        return null; 
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

        var updatedProperty = DMShopRepository.UpdateProperty(property);
        return new PropertyDto { Id = updatedProperty.Id, PropertyName = updatedProperty.PropertyName };
    }

    public List<OrderListDto> GetOrdersForList(int limit, int startAt)
    {
        try
        {
            var orders = DMShopRepository.GetOrdersForList(limit, startAt);
            return orders.Select(order => OrderListDto.FromEntity(order)).ToList();
        }
        catch (DataAccessException)
        {
            throw new ServiceException("Failed to retrieve orders from the service layer");
        }    
    }

    public OrderDto CreateOrder(CreateOrderDTO createOrderDto)
    {
        double totalAmount = 0;
        
        var paperIds = createOrderDto.OrderEntries.Select(o => o.ProductId).ToList();
        var papers = DMShopRepository.GetPaperByIds(paperIds);

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
            DeliveryDate = createOrderDto.DeliveryDate.Date != null 
                ? DateOnly.FromDateTime(createOrderDto.DeliveryDate) 
                : (DateOnly?)null,
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
        var createdOrder = DMShopRepository.CreateOrder(order, orderEntries);
        return OrderDto.FromEntity(createdOrder);
    }
    

    public OrderDetailsDto GetOrderDetailsById(int orderId)
    {
        try
        {
            var order = DMShopRepository.GetOrderDetailsById(orderId);
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
            var customer = DMShopRepository.GetRandomCustomer();
            if (customer == null) throw new ServiceException("No customers found.");

            var orders = DMShopRepository.GetOrdersForCustomer(customer.Id);
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
            var updateStatusDto = new UpdateOrderStatusDTO { newStatus = newStatus };
            updateOrderStatusValidator.ValidateAndThrow(updateStatusDto);
            
            var updatedOrder = DMShopRepository.UpdateOrderStatus(orderId, newStatus);
            
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