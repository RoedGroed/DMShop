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
    }
}
    