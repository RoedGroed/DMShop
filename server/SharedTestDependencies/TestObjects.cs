using Bogus;
using DataAccess.Models;

namespace SharedTestDependencies;

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
       Customer customer = null;
        customer = GetCustomer();

        return new Faker<Order>()
            .RuleFor(o => o.OrderDate, f => f.Date.Past())
            .RuleFor(o => o.DeliveryDate, f => f.Date.PastDateOnly())
            .RuleFor(o => o.Status, f => f.PickRandom(new[] { "pending", "delivered", "processing", "cancelled" }))
            .RuleFor(o => o.TotalAmount, f => f.Random.Double(50, 500))
            .RuleFor(o => o.Customer, _ => customer)
            .RuleFor(o => o.CustomerId, _ => customer.Id)
            .RuleFor(o => o.OrderEntries, f => new List<OrderEntry>());
    }

}