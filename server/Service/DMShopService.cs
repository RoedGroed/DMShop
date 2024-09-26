using DataAccess.Interfaces;
using Microsoft.Extensions.Logging;
using Service.TransferModels.Responses;

namespace Service;

public interface IDMShopService
{
    public List<OrderDto> GetAllOrders(int limit, int startAt);
}


public class DMShopService(IDMShopRepository dmShopRepository) :IDMShopService
{
    public List<OrderDto> GetAllOrders(int limit, int startAt)
    {
        var orders = dmShopRepository.GetAllOrders(limit, startAt);
        return orders.Select(order => OrderDto.FromEntity(order)).ToList();
    }
}

