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

    [HttpGet]
    [Route("{orderId}")]
    public ActionResult<OrderDetailsDto> GetOrderById(int orderId)
    {
        var orderDetail = service.GetOrderDetailsById(orderId);
        return Ok(orderDetail);
    }

    [HttpPut]
    [Route("{orderId}/status")]
    public IActionResult UpdateOrderStatus(int orderId, [FromBody] UpdateOrderStatusDTO request)
    {
        var updatedOrder = service.UpdateOrderStatus(orderId, request.newStatus);
        return Ok(OrderDetailsDto.FromEntity(updatedOrder));
    }

}