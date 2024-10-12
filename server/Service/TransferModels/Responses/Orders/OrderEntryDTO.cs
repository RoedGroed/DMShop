using DataAccess.Models;

namespace Service.TransferModels.Responses.Orders
{
    public class OrderEntryDto
    {
        public int Id { get; set; }
        public int Quantity { get; set; }
        public int? ProductId { get; set; }
        public string? ProductName { get; set; }
        public double? Price { get; set; }
        public double TotalPrice { get; set; }

        public static OrderEntryDto FromEntity(OrderEntry orderEntry)
        {
            return new OrderEntryDto
            { 
                Quantity = orderEntry.Quantity, 
                ProductId = orderEntry.ProductId,
                ProductName = orderEntry.Product?.Name,
                Price = orderEntry.Product?.Price,
                TotalPrice = orderEntry.Product!.Price * orderEntry.Quantity
            };
        }
    }
}
