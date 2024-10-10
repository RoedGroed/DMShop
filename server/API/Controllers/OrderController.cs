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
        try
        {
            var orders = service.GetOrdersForList(limit, startAt);  
            return Ok(orders);
        }
        catch (ServiceException)
        {
            return StatusCode(500, new { error = "An error occurred while retrieving orders" });
        }    
    }

    [HttpGet]
    [Route("{orderId}")]
    public ActionResult<OrderDetailsDto> GetOrderById(int orderId)
    {
        var orderDetail = service.GetOrderDetailsById(orderId);
        return Ok(orderDetail);
    }
    
    [HttpGet]
    [Route("random-customer/orders")]
    public ActionResult<List<OrderListDto>> GetRandomCustomerOrderHistory()
    {
        var orders = service.GetRandomCustomerOrderHistory();
        if (!orders.Any())
        {
            return NotFound("No orders found for any customer");
        }
        return Ok(orders);
    }

    [HttpPut]
    [Route("{orderId}/status")]
    public IActionResult UpdateOrderStatus(int orderId, [FromBody] UpdateOrderStatusDTO request)
    {
        try
        {
            var updatedOrder = service.UpdateOrderStatus(orderId, request.newStatus);
            return Ok(OrderDetailsDto.FromEntity(updatedOrder));
        }
        catch (ServiceException ex)
        {
            return StatusCode(500, new { message = ex.Message });
        }
        catch (Exception ex) 
        {
            return StatusCode(500, new { message = "An unexpected error occurred" });
        }    
    }

}