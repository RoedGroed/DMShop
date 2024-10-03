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

        public static Paper GetPaper()
        {
            return new Faker<Paper>()
                .RuleFor(p => p.Name, f => f.Commerce.ProductName())
                .RuleFor(p => p.Price, f => f.Random.Double(1.0, 100.0))
                .RuleFor(p => p.Stock, f => f.Random.Int(0, 1000))
                .RuleFor(p => p.Discontinued, f => f.Random.Bool());
        }

        public static Property GetProperty()
        {
            return new Faker<Property>()
                .RuleFor(p => p.PropertyName, f => f.Commerce.ProductAdjective());
        }

        public static Order GetOrder()
        {
            return new Faker<Order>()
                .RuleFor(o => o.OrderDate, _ => DateTime.UtcNow.AddMilliseconds(-DateTime.UtcNow.Millisecond).AddTicks(-(DateTime.UtcNow.Ticks % TimeSpan.TicksPerSecond))) // Trim milliseconds and ticks
                .RuleFor(o => o.DeliveryDate, _ => DateOnly.FromDateTime(DateTime.UtcNow.AddDays(5)))
                .RuleFor(o => o.Status, f => f.PickRandom(new[] { "pending", "delivered", "cancelled", "processing" }))
                .RuleFor(o => o.TotalAmount, f => f.Random.Double(10.0, 500.0))
                .RuleFor(o => o.CustomerId, _ => 1) // Placeholder for customer ID
                .RuleFor(o => o.Customer, GetCustomer())
                .RuleFor(o => o.OrderEntries, new List<OrderEntry>());
        }





        public static OrderEntry GetOrderEntry(Paper product, Order order)
        {
            return new Faker<OrderEntry>()
                .RuleFor(oe => oe.Quantity, f => f.Random.Int(1, 20))
                .RuleFor(oe => oe.ProductId, f => product.Id)
                .RuleFor(oe => oe.OrderId, f => order.Id)
                .RuleFor(oe => oe.Product, product)
                .RuleFor(oe => oe.Order, order);
        }

        public static List<OrderEntry> GetOrderEntries(Order order)
        {
            var papers = new List<Paper> { GetPaper(), GetPaper(), GetPaper() }; // 3 random products
            var orderEntries = new List<OrderEntry>();

            foreach (var paper in papers)
            {
                var orderEntry = GetOrderEntry(paper, order);
                orderEntries.Add(orderEntry);
            }

            return orderEntries;
        }

        public static Order GetOrderWithEntries()
        {
            var order = GetOrder();
            order.OrderEntries = GetOrderEntries(order);
            return order;
        }
    }
}
    