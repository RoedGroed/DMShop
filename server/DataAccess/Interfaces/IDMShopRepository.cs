using DataAccess.Models;

namespace DataAccess.Interfaces;

public interface IDMShopRepository
{
    // outline the methods for the repo
    Order CreateOrder(Order order, List<OrderEntry> orderEntries);
    List<Paper> GetPaperByIds(List<int> productIds);
    List<Paper> GetAllPapers();
    List<Paper> GetAllPapersWithProperties();
    Paper GetPaperById(int id);
    Paper CreatePaper(Paper paper, List<int> propertyIds);
    Paper DeletePaper(int id, List<int> propertyIds);
    Paper UpdatePaper(Paper paper, List<int> propertyIds);
    
    Property CreateProperty(Property property);
    void DeleteProperty(int propertyId);
    Property UpdateProperty(Property updatedProperty);

    List<Property> GetAllProperties();
    void AddPropertiesToPaper(int paperId, List<int> propertyIds);
    
    public List<Order> GetOrdersForList(int limit, int startAt);

    public Order GetOrderDetailsById(int orderId);
    
    public List<Order> GetOrdersForCustomer(int customerId);
    public Order UpdateOrderStatus(int orderId, string newStatus);
    public Customer GetRandomCustomer();
}