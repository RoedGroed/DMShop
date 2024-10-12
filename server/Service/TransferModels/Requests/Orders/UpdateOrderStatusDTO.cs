namespace Service.TransferModels.Requests.Orders;

public class UpdateOrderStatusDto
{
    public required string NewStatus { get; set; }
}