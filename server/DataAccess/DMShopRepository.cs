using DataAccess.Interfaces;
using DataAccess.Models;
using Microsoft.EntityFrameworkCore;

namespace DataAccess;

public class DMShopRepository (DMShopContext context) : IDMShopRepository
{
    // Implement the methods from the interface here.

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



    public Paper CreatePaper(Paper paper)
    {
        context.Papers.Add(paper);
        context.SaveChanges();
        return paper;
    }

    public Paper DeletePaper(int id)
    {
        var paper = context.Papers.Find(id);
        if (paper == null)
        {
            throw new ArgumentException($"Paper with id {id} not found.");
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
        
        existingPaper.Name = paper.Name;
        existingPaper.Price = paper.Price;
        existingPaper.Stock = paper.Stock;
        existingPaper.Discontinued = paper.Discontinued;
        
        // Update paper properties
        var updatedProperties = context.Properties.Where(p => propertyIds.Contains(p.Id)).ToList();

        // Clear the existing properties and add the new set of properties
        existingPaper.Properties.Clear();
        foreach (var property in updatedProperties)
        {
            existingPaper.Properties.Add(property);
        }

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

}