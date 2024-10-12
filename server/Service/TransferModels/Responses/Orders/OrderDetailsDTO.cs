using DataAccess.Models;

namespace Service.TransferModels.Responses.Orders
{
    public class OrderDetailsDto
    {
        public int Id { get; set; }
        public DateTime OrderDate { get; set; }
        public DateOnly? DeliveryDate { get; set; }
        public required string Status { get; set; }
        public double TotalAmount { get; set; }
        
        public required string CustomerName { get; set; }
        public required string CustomerAddress { get; set; }
        public string? CustomerEmail { get; set; }
        public required string CustomerPhone { get; set; }

        public List<OrderEntryDto> OrderEntries { get; set; } = new();

        public static OrderDetailsDto FromEntity(Order order)
        {
            return new OrderDetailsDto
            {
                Id = order.Id,
                OrderDate = order.OrderDate,
                DeliveryDate = order.DeliveryDate,
                Status = order.Status,
                TotalAmount = order.TotalAmount,
                
                CustomerName = order.Customer?.Name!,
                CustomerAddress = order.Customer?.Address!,
                CustomerEmail = order.Customer?.Email,
                CustomerPhone = order.Customer?.Phone!,
                
                OrderEntries = order.OrderEntries.Select(OrderEntryDto.FromEntity).ToList()
            };
        }
    }
}