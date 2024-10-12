using DataAccess.Models;

namespace Service.TransferModels.Responses.Orders
{
    public class OrderListDto
    {
        public int Id { get; set; }
        public int? CustomerId { get; set; }
        public required string CustomerName { get; set; }
        public DateTime OrderDate { get; set; }
        public DateOnly? DeliveryDate { get; set; }
        public double TotalAmount { get; set; }
        public required string Status { get; set; }

        public static OrderListDto FromEntity(Order order)
        {
            return new OrderListDto
            {
                Id = order.Id,
                CustomerId = order.CustomerId,
                CustomerName = order.Customer?.Name ?? string.Empty,
                OrderDate = order.OrderDate,
                DeliveryDate = order.DeliveryDate,
                TotalAmount = order.TotalAmount,
                Status = order.Status
            };
        }
    }
}
