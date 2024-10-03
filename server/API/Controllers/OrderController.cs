using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Service;
using Service.TransferModels.Responses;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OrderController(IDMShopService service, IOptionsMonitor<AppOptions> options) : ControllerBase
{


    [HttpGet]
    [Route("")]
    public ActionResult<List<OrderListDto>> GetOrdersForList(int limit = 10, int startAt = 0)
    {
        var orders = service.GetOrdersForList(limit, startAt);  
        return Ok(orders);
    }
}