using DataAccess.Models;

namespace Service.TransferModels.Responses;

public class OrderDto
{
    public int Id { get; set; } 
    public int? CustomerId { get; set; } 
    public DateTime? OrderDate { get; set; } 
    public DateTime? DeliveryDate { get; set; } 
    public string Status { get; set; } 
    public double TotalAmount { get; set; } 
    public List<OrderEntryDto> OrderEntries { get; set; } 

    // Static method to map from entity to DTO
    public static OrderDto FromEntity(Order order)
    {
        return new OrderDto
        {
            Id = order.Id, 
            CustomerId = order.CustomerId,
            OrderDate = order.OrderDate, 
            DeliveryDate = order.DeliveryDate.HasValue 
                ? (DateTime?)order.DeliveryDate.Value.ToDateTime(new TimeOnly()) 
                : null, 
            Status = order.Status,
            TotalAmount = order.TotalAmount,
            OrderEntries = order.OrderEntries != null
                ? order.OrderEntries.Select(entry => new OrderEntryDto
                {
                    ProductId = entry.ProductId,
                    Quantity = entry.Quantity
                }).ToList()
                : new List<OrderEntryDto>() 
        };
    }
}