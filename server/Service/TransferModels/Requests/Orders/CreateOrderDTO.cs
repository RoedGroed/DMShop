using Service.TransferModels.Requests;

namespace Service.TransferModels.Requests;

public class CreateOrderDTO
{
    public int CustomerId { get; set; }
    public List<OrderEntryRequestDTO> Items { get; set; }
    public DateTime DeliveryDate { get; set; }
    public DateTime OrderDate { get; set; }
    public string Status { get; set; }
    public List<OrderEntryRequestDTO> OrderEntries { get; set; }
    public double TotalAmount { get; set; }
}
