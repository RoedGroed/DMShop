﻿using DataAccess.Interfaces;
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

    public List<Paper> GetPapersByProperties(List<int> propertyIds)
    {
        return context.Papers.Where(p => p.Properties.Any(prop => propertyIds.Contains(prop.Id))).ToList();
    }
}