using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Service;
using Service.TransferModels.Responses;


namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PropertyController(IDMShopService service, IOptionsMonitor<AppOptions> options) : ControllerBase
{

    [HttpGet]
    [Route("")]
    public ActionResult<List<PropertyDto>> GetAllProperties()
    {
        var properties = service.GetAllProperties();
        if (properties == null || !properties.Any())
        {
            return NotFound("No properties found");
        }
    
        return Ok(properties);
    }
}