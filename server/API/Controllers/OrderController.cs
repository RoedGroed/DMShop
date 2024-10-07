using DataAccess;
using DataAccess.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Service;
using Service.TransferModels.Requests;
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

    [HttpPost]
    [Route("create")]
    public ActionResult<OrderDto> createOrder([FromBody] CreateOrderDTO createOrderDto)
    {
        if (createOrderDto == null)
        {
            return BadRequest("Invalid order data");
        }
        var createdOrder = service.CreateOrder(createOrderDto);
        return CreatedAtAction(nameof(GetOrdersForList), new { id = createdOrder.Id }, createdOrder);
    }
}