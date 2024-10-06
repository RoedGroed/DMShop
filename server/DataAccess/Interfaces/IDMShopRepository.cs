using DataAccess.Models;

namespace DataAccess.Interfaces;

public interface IDMShopRepository
{
    // outline the methods for the repo
    List<PaperApi> GetAllPapers();
    List<PaperApi> GetAllPapersWithProperties();
    PaperApi GetPaperById(int id);
    PaperApi CreatePaper(PaperApi paperApi, List<int> propertyIds);
    PaperApi DeletePaper(int id, List<int> propertyIds);
    PaperApi UpdatePaper(PaperApi paperApi, List<int> propertyIds);

    List<Property> GetAllProperties();
    void AddPropertiesToPaper(int paperId, List<int> propertyIds);
    
    public List<Order> GetOrdersForList(int limit, int startAt);

    public Order GetOrderDetailsById(int orderId);
}