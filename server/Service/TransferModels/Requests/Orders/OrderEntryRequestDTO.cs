namespace Service.TransferModels.Requests.Orders;

public class OrderEntryRequestDto
{
    public int ProductId { get; set; }
    public int Quantity { get; set; }
}