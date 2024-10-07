using Service.TransferModels.Responses;

namespace Service.TransferModels.Requests;

public class CreateOrderDTO
{
    public int CustomerId { get; set; }
    public List<OrderEntryDto> Items { get; set; }
    public DateTime DeliveryDate { get; set; }
    public DateTime OrderDate { get; set; }
    public string Status { get; set; }
    public List<OrderEntryDto> OrderEntries { get; set; }
    public double TotalAmount { get; set; }
}
public class OrderEntryDto
{
    public int ProductId { get; set; }
    public int Quantity { get; set; }
}