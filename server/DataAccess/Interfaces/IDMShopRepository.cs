using DataAccess.Models;

namespace DataAccess.Interfaces;

public interface IDMShopRepository
{
    // outline the methods for the repo
    List<Paper> GetAllPapers();
    List<Paper> GetAllPapersWithProperties();
    Paper GetPaperById(int id);
    Paper CreatePaper(Paper paper, List<int> propertyIds);
    Paper DeletePaper(int id, List<int> propertyIds);
    Paper UpdatePaper(Paper paper, List<int> propertyIds);

    List<Property> GetAllProperties();
    void AddPropertiesToPaper(int paperId, List<int> propertyIds);
    
    public List<Order> GetOrdersForList(int limit, int startAt);

    public Order GetOrderDetailsById(int orderId);
}