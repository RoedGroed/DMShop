using DataAccess.Interfaces;
using DataAccess.Models;

namespace DataAccess;

public class DMShopRepository (DMShopContext context) : IDMShopRepository
{
    // Implement the methods from the interface here.

    public List<Paper> GetAllPapers()
    {
        return context.Papers.ToList();
    }

    public List<Paper> GetAllPapersWithProperties()
    {
        throw new NotImplementedException();
    }

    public Paper CreatePaper(Paper paper)
    {
        throw new NotImplementedException();
    }
}