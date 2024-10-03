using DataAccess.Models;

namespace Service.TransferModels.Responses
{
    public class OrderDetailsDto
    {
        public int Id { get; set; }
        public DateTime OrderDate { get; set; }
        public DateOnly? DeliveryDate { get; set; }
        public string Status { get; set; }
        public double TotalAmount { get; set; }
        public string CustomerName { get; set; }
        public string CustomerEmail { get; set; }
        public string CustomerPhone { get; set; }

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
                CustomerName = order.Customer?.Name ?? "Unknown",
                CustomerEmail = order.Customer?.Email ?? "Unknown",
                CustomerPhone = order.Customer?.Phone ?? "Unknown",
                OrderEntries = order.OrderEntries.Select(OrderEntryDto.FromEntity).ToList()
            };
        }
    }
}