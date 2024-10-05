using Bogus;
using System;
using System.Linq;
using DataAccess.Models;

namespace DataAccess
{
    public class MockDataSeeder
    {
        public static void SeedPapers(DMShopContext context, int paperCount, int propertyCount)
        {
            // Generate fake papers
            var paperFaker = new Faker<Paper>()
                .RuleFor(p => p.Name, f => f.Commerce.ProductName())
                .RuleFor(p => p.Discontinued, f => f.Random.Bool())
                .RuleFor(p => p.Stock, f => f.Random.Int(0, 1000))
                .RuleFor(p => p.Price, f => f.Random.Double(5.0, 50.0));

            var papers = paperFaker.Generate(paperCount);

            // Generate fake properties
            var propertyFaker = new Faker<Property>()
                .RuleFor(p => p.PropertyName, f => f.Commerce.ProductAdjective());

            var properties = propertyFaker.Generate(propertyCount);

            // Associate random properties with papers
            foreach (var paper in papers)
            {
                var randomProperties = properties.OrderBy(p => Guid.NewGuid()).Take(2).ToList();
                paper.Properties = randomProperties; // Correctly associating properties
            }

            context.Papers.AddRange(papers);
            context.Properties.AddRange(properties);
            context.SaveChanges(); // Save both papers and properties to the database
        }

    }
}