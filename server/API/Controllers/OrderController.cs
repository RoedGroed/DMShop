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