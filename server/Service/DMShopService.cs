using DataAccess.Interfaces;
using DataAccess.Models;
using Service.TransferModels.Responses;
using Microsoft.Extensions.Logging;

namespace Service;

public interface IDMShopService
{
    public List<ProductDto> GetAllPapers();
    
    public List<ProductDto> GetAllPapersWithProperties();

    public ProductDto CreatePaper(ProductDto productDto);

    public ProductDto DeletePaper(int id);

    public ProductDto UpdatePaper(int id, ProductDto productDto, List<int> propertyIds);
 
    public List<OrderDto> GetAllOrders(int limit, int startAt);
}


public class DMShopService(IDMShopRepository DMShopRepository) :IDMShopService
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
        var paper = new Paper
        {
            Name = productDto.Name,
            Price = productDto.Price,
            Stock = productDto.Stock,
            Discontinued = productDto.Discontinued = false
        };

        var createdPaper = DMShopRepository.CreatePaper(paper);

        if (productDto.Properties != null && productDto.Properties.Any())
        {
            var propertyIds = productDto.Properties.Select(p => p.Id).ToList();
            DMShopRepository.AddPropertiesToPaper(createdPaper.Id, propertyIds);
        }

        return ProductDto.FromEntity(createdPaper);
    }

    public ProductDto DeletePaper(int id)
    {
        var deletedPaper = DMShopRepository.DeletePaper(id);
        return ProductDto.FromEntity(deletedPaper);

    }

    public ProductDto UpdatePaper(int id, ProductDto productDto, List<int> propertyIds)
    {

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

    public List<OrderDto> GetAllOrders(int limit, int startAt)
    {
        var orders = DMShopRepository.GetAllOrders(limit, startAt);
        return orders.Select(order => OrderDto.FromEntity(order)).ToList();
    }
}