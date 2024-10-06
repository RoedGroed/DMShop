using DataAccess.Interfaces;
using DataAccess.Models;
using Microsoft.EntityFrameworkCore;

namespace DataAccess;

public class DMShopRepository(DMShopContext context) : IDMShopRepository
{
    public List<PaperApi> GetAllPapers()
    {
        return context.Papers.ToList();
    }

    public List<PaperApi> GetAllPapersWithProperties()
    {
        return context.Papers
            .Include(p => p.OrderEntries)
            .Include(p => p.Properties)
            .ToList();
    }

    public PaperApi GetPaperById(int id)
    {
        return context.Papers.Include(p => p.Properties).FirstOrDefault(p => p.Id == id);
    }


    public PaperApi CreatePaper(PaperApi paperApi, List<int> propertyIds)
    {
        // Fetch the properties to be associated with the paper
        var properties = context.Properties.Where(p => propertyIds.Contains(p.Id)).ToList();

        // Add the properties to the paper object
        paperApi.Properties = properties;

        // Add the paper to the context
        context.Papers.Add(paperApi);
        context.SaveChanges();

        return paperApi;
    }

    public PaperApi DeletePaper(int id, List<int> propertyIds)
    {
        // Retrieve the paper along with its associated properties
        var paper = context.Papers
            .Include(p => p.Properties)
            .FirstOrDefault(p => p.Id == id);

        if (paper == null)
        {
            throw new ArgumentException($"Paper with id {id} not found.");
        }

        // Remove associated properties based on provided propertyIds
        var propertiesToRemove = paper.Properties
            .Where(p => propertyIds.Contains(p.Id))
            .ToList();

        // Remove the selected properties from the paper
        foreach (var property in propertiesToRemove)
        {
            paper.Properties.Remove(property);
        }

        context.Papers.Remove(paper);
        context.SaveChanges();

        return paper;
    }

    public PaperApi UpdatePaper(PaperApi paperApi, List<int> propertyIds)
    {
        var existingPaper = context.Papers
            .Include(p => p.Properties)
            .FirstOrDefault(p => p.Id == paperApi.Id);

        if (existingPaper == null)
        {
            throw new ArgumentException($"Paper with id {paperApi.Id} not found.");
        }

        // Update the paper's basic properties
        existingPaper.Name = paperApi.Name;
        existingPaper.Price = paperApi.Price;
        existingPaper.Stock = paperApi.Stock;
        existingPaper.Discontinued = paperApi.Discontinued;

        // Update paper properties
        var updatedProperties = context.Properties.Where(p => propertyIds.Contains(p.Id)).ToList();

        // Reassign properties in a single step
        existingPaper.Properties = updatedProperties;

        context.Papers.Update(existingPaper);
        context.SaveChanges();

        return existingPaper;
    }

    public void AddPropertiesToPaper(int paperId, List<int> propertyIds)
    {
        var paper = context.Papers.Include(p => p.Properties).FirstOrDefault(p => p.Id == paperId);

        var properties = context.Properties.Where(p => propertyIds.Contains(p.Id)).ToList();
        foreach (var property in properties)
        {
            paper.Properties.Add(property);
        }

        context.SaveChanges();
    }

    public List<Property> GetAllProperties()
    {
        return context.Properties.ToList();
    }

    public List<Order> GetOrdersForList(int limit, int startAt)
    {
        return context.Orders
            .Include(o => o.Customer)
            .OrderBy(o => o.Id)
            .Skip(startAt)
            .Take(limit)
            .ToList();
    }

    public Order GetOrderDetailsById(int orderId)
    {
        var order = context.Orders
            .Include(o => o.OrderEntries)
                .ThenInclude(oe => oe.Product)
            .Include(o => o.Customer)
            .FirstOrDefault(o => o.Id == orderId);

        return order;
    }
    
}