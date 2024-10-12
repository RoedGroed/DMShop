namespace Service.TransferModels.Requests.Orders;

public class CreateOrderDto
{
    public int CustomerId { get; set; }
    public DateTime DeliveryDate { get; set; }
    public DateTime OrderDate { get; set; }
    public required string Status { get; set; }
    public required List<OrderEntryRequestDto> OrderEntries { get; set; }
    public double TotalAmount { get; set; }
}
