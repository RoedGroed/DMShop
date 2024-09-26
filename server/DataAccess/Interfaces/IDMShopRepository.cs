using DataAccess.Models;

namespace DataAccess.Interfaces;

public interface IDMShopRepository
{
    public List<Order> GetAllOrders(int limit, int startAt);
}