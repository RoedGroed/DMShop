using DataAccess.Models;

namespace DataAccess.Interfaces;

public interface IDMShopRepository
{
    public List<Paper> GetAllPapers();

    public List<Paper> GetAllPapersWithProperties();

    public Paper CreatePaper(Paper paper);

    public Paper DeletePaper(int id);

    public Paper UpdatePaper(Paper paper, List<int> propertyIds);
    void AddPropertiesToPaper(int paperId, List<int> propertyIds);
    
    public List<Order> GetOrdersForList(int limit, int startAt);

    public Order GetOrderDetailsById(int orderId);
}