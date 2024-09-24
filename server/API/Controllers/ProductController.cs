using DataAccess.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Service;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductController(IDMShopService service, IOptionsMonitor<AppOptions> options) : ControllerBase
{

    [HttpGet]
    [Route("")]
    public ActionResult<List<Paper>> GetAllPapers()
    {
        var papers = service.GetAllPapers();
        return Ok(papers);
    }
    
}