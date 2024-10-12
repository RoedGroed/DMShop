using Microsoft.AspNetCore.Mvc;
using Service;
using Service.TransferModels.Requests.Orders;
using Service.TransferModels.Responses.Orders;

namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OrderController(IDmShopService service) : ControllerBase
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
        catch (ServiceException ex)
        {
            return StatusCode(500, new { error = ex.Message });
        }    
    }

    [HttpGet]
    [Route("{orderId}")]
    public ActionResult<OrderDetailsDto> GetOrderById(int orderId)
    {
        try
        {
            var orderDetail = service.GetOrderDetailsById(orderId);
            return Ok(orderDetail);
        }
        catch (ServiceException ex)
        {
            return StatusCode(500, new { error = ex.Message });
        }
    }
    
    [HttpGet]
    [Route("random-customer/orders")]
    public ActionResult<List<OrderListDto>> GetRandomCustomerOrderHistory()
    {
        try
        {
            var orders = service.GetRandomCustomerOrderHistory();
            return Ok(orders);
        }
        catch (ServiceException ex)
        {
            return StatusCode(500, new { error = ex.Message });
        }
    }

    [HttpPut]
    [Route("{orderId}/status")]
    public IActionResult UpdateOrderStatus(int orderId, [FromBody] UpdateOrderStatusDto request)
    {
        try
        {
            var updatedOrder = service.UpdateOrderStatus(orderId, request.NewStatus);
            return Ok(OrderDetailsDto.FromEntity(updatedOrder));
        }
        catch (ServiceException ex)
        {
            return StatusCode(500, new { message = ex.Message });
        }
    }
    
    [HttpPost]
    [Route("create")]
    public ActionResult<OrderDto> CreateOrder([FromBody] CreateOrderDto createOrderDto)
    {
        var createdOrder = service.CreateOrder(createOrderDto);
        return CreatedAtAction(nameof(GetOrdersForList), new { id = createdOrder.Id }, createdOrder);
    }
}