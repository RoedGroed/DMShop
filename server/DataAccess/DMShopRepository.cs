using DataAccess.Interfaces;
using DataAccess.Models;
using Microsoft.EntityFrameworkCore;

namespace DataAccess;

public class DMShopRepository(DMShopContext context) : IDMShopRepository
{
    public List<Order> GetAllOrders(int limit, int startAt)
    {
        return context.Orders
            .Include(o => o.OrderEntries)
            .Include(o => o.Customer)
            .OrderBy(o => o.Id)
            .Skip(startAt)
            .Take(limit)
            .ToList();
    }
    
}