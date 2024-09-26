using DataAccess.Models;

namespace Service.TransferModels.Responses
{
    public class OrderDto
    {
        public int Id { get; set; }
        public DateTime OrderDate { get; set; }
        public DateOnly? DeliveryDate { get; set; }
        public string Status { get; set; } = null!;
        public double TotalAmount { get; set; }
        public int? CustomerId { get; set; }
        public string? CustomerName { get; set; }
        public List<OrderEntryDto> OrderEntries { get; set; } = new List<OrderEntryDto>();

        public static OrderDto FromEntity(Order order)
        {
            return new OrderDto
            {
                Id = order.Id,
                OrderDate = order.OrderDate,
                DeliveryDate = order.DeliveryDate,
                Status = order.Status,
                TotalAmount = order.TotalAmount,
                CustomerId = order.CustomerId,
                CustomerName = order.Customer?.Name ?? string.Empty,
                OrderEntries = order.OrderEntries.Select(entry => OrderEntryDto.FromEntity(entry)).ToList()
            };
        }
    }
}