using DataAccess;
using DataAccess.Interfaces;
using DataAccess.Models;

namespace Service;

public interface IDMShopService
{
    public List<Paper> GetAllPapers();
}


public class DMShopService(
    IDMShopRepository DMShopRepository) : IDMShopService
{
    public List<Paper> GetAllPapers()
    {
        return DMShopRepository.GetAllPapers();
    }
}