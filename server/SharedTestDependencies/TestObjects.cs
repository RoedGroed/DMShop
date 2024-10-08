using Bogus;
using DataAccess.Models;

namespace SharedTestDependencies
{
    public class TestObjects
    {
        public static Customer GetCustomer()
        {
            return new Faker<Customer>()
                .RuleFor(c => c.Name, f => f.Name.FullName())
                .RuleFor(c => c.Address, f => f.Address.FullAddress())
                .RuleFor(c => c.Email, f => f.Internet.Email())
                .RuleFor(c => c.Phone, f => f.Phone.PhoneNumber());
        }

        public static Order GetOrder()
        {
            return new Faker<Order>()
                .RuleFor(o => o.OrderDate, _ => DateTime.UtcNow.AddMilliseconds(-DateTime.UtcNow.Millisecond))
                .RuleFor(o => o.DeliveryDate, _ => DateOnly.FromDateTime(DateTime.UtcNow.AddDays(5)))
                .RuleFor(o => o.Status, f => f.PickRandom(new[] { "pending", "delivered", "cancelled", "processing" }))
                .RuleFor(o => o.TotalAmount, f => f.Random.Double(10.0, 500.0))
                .RuleFor(o => o.CustomerId, _ => 1) // Placeholder for customer ID
                .RuleFor(o => o.Customer,GetCustomer())
                .RuleFor(o => o.OrderEntries, new List<OrderEntry>());
        }

        public static List<Paper> GetPapers(int paperCount, int propertyCount)
        {
            var papers = GeneratePapers(paperCount);
            var properties = GenerateProperties(propertyCount);

            // Associate random properties with each paper
            foreach (var paper in papers)
            {
                var randomProperties = properties.OrderBy(p => Guid.NewGuid()).Take(2).ToList();
                paper.Properties = randomProperties;
            }

            return papers;
        }
        
        // Helper to create a list of papers
        private static List<Paper> GeneratePapers(int count)
        {
            var paperFaker = new Faker<Paper>()
                .RuleFor(p => p.Name, f => f.Commerce.ProductName())
                .RuleFor(p => p.Discontinued, f => f.Random.Bool())
                .RuleFor(p => p.Stock, f => f.Random.Int(0, 1000))
                .RuleFor(p => p.Price, f => f.Random.Double(5.0, 50.0));

            return paperFaker.Generate(count);
        }

        // Helper to create a list of properties
        private static List<Property> GenerateProperties(int count)
        {
            var propertyFaker = new Faker<Property>()
                .RuleFor(p => p.PropertyName, f => f.Commerce.ProductAdjective());

            return propertyFaker.Generate(count);
        }
        
        public static List<Property> GetProperties(int count)
        {
            return new Faker<Property>()
                .RuleFor(p => p.PropertyName, f => f.Commerce.ProductAdjective())
                .Generate(count);
        }
        
        
        
    }
}
    