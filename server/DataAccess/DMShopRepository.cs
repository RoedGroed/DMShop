using DataAccess.Interfaces;
using DataAccess.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;

namespace DataAccess;

public class DMShopRepository(DMShopContext context) : IDMShopRepository
{
    public List<Paper> GetAllPapers()
    {
        return context.Papers.ToList();
    }

    public List<Paper> GetAllPapersWithProperties()
    {
        return context.Papers
            .Include(p => p.OrderEntries)
            .Include(p => p.Properties)
            .ToList();
    }

    public Paper GetPaperById(int id)
    {
        return context.Papers.Include(p => p.Properties).FirstOrDefault(p => p.Id == id);
    }


    public Paper CreatePaper(Paper paper, List<int> propertyIds)
    {
        // Fetch the properties to be associated with the paper
        var properties = context.Properties.Where(p => propertyIds.Contains(p.Id)).ToList();

        // Add the properties to the paper object
        paper.Properties = properties;

        // Add the paper to the context
        context.Papers.Add(paper);
        context.SaveChanges();

        return paper;
    }

    public Paper DeletePaper(int id, List<int> propertyIds)
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

    public Paper UpdatePaper(Paper paper, List<int> propertyIds)
    {
        var existingPaper = context.Papers
            .Include(p => p.Properties)
            .FirstOrDefault(p => p.Id == paper.Id);

        if (existingPaper == null)
        {
            throw new ArgumentException($"Paper with id {paper.Id} not found.");
        }

        // Update the paper's basic properties
        existingPaper.Name = paper.Name;
        existingPaper.Price = paper.Price;
        existingPaper.Stock = paper.Stock;
        existingPaper.Discontinued = paper.Discontinued;

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
    
    
    public Property CreateProperty(Property property)
    {
        context.Properties.Add(property);
        context.SaveChanges();

        return property;
    }

    public void DeleteProperty(int propertyId)
    {
        // Remove associations in the join table.
        var property = context.Properties
            .Include(p => p.Papers)  // Load related papers
            .FirstOrDefault(p => p.Id == propertyId);
        
        // Remove the associations with the paper
        property.Papers.Clear();
        
        // remove the property in its own table
        context.Properties.Remove(property);
        context.SaveChanges();
    }

    public Property UpdateProperty(Property updatedProperty)
    {
        var existingProperty = context.Properties.FirstOrDefault(p => p.Id == updatedProperty.Id);
        
        existingProperty.PropertyName = updatedProperty.PropertyName;

        context.SaveChanges();
        return existingProperty;
    }
    
    
    public List<Order> GetOrdersForList(int limit, int startAt)
    {
        try
        {
            return context.Orders
                .Include(o => o.Customer)
                .OrderBy(o => o.Id)
                .Skip(startAt)
                .Take(limit)
                .ToList();
        }
        catch (Exception ex)
        {
            throw new DataAccessException("Failed to retrieve orders from the database");
        }
    }

    public Order GetOrderDetailsById(int orderId)
    {
        try
        {
            var order = context.Orders
                .Include(o => o.OrderEntries)
                    .ThenInclude(oe => oe.Product)
                .Include(o => o.Customer)
                .FirstOrDefault(o => o.Id == orderId);

            return order;
        }
        catch (Exception ex)
        {
            throw new DataAccessException($"Failed to retrieve order with ID {orderId}");
        }    
    }

    public Order UpdateOrderStatus(int orderId, string newStatus)
    {
        try
        {
            var order = context.Orders.FirstOrDefault(o => o.Id == orderId);
            
            order.Status = newStatus;
            context.SaveChanges();
            
            return order;
        }
        catch (Exception)
        {
            throw new DataAccessException("Failed to update the order status in the database");
        }
    }

    public Order CreateOrder(Order order, List<OrderEntry> orderEntries)
    {
        context.Orders.Add(order);
        context.OrderEntries.AddRange(orderEntries);
        context.SaveChanges();
        return order;
    }

    public List<Paper> GetPaperByIds(List<int> paperIds)
    {
        return context.Papers.Where(paper => paperIds.Contains(paper.Id)).ToList();
    }
    

    public List<Order> GetOrdersForCustomer(int customerId)
    {
        return context.Orders
            .Include(o => o.Customer)
            .Where(o => o.CustomerId == customerId)
            .OrderByDescending(o => o.OrderDate)
            .ToList();
    }
    
    public Customer GetRandomCustomer()
    {
        try
        {
            int customerCount = context.Customers.Count();
            if (customerCount == 0) return null;

            int randomIndex = new Random().Next(customerCount);
            return context.Customers.Skip(randomIndex).FirstOrDefault();
        }
        catch (Exception ex)
        {
            throw new DataAccessException("Failed to retrieve a random customer");
        }    
    }
}