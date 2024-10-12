using FluentValidation;
using Service.TransferModels.Requests.Orders;


namespace Service.Validators;

public class UpdateOrderStatusValidator : AbstractValidator<UpdateOrderStatusDto>
{
    public UpdateOrderStatusValidator()
    {
        var validStatuses = new[] { "pending", "processing", "delivered", "cancelled" };

        RuleFor(x => x.NewStatus)
            .NotEmpty().WithMessage("Status cannot be empty")
            .Must(status => validStatuses.Contains(status))
            .WithMessage("Invalid status. Valid statuses are: pending, processing, delivered, cancelled");
    }
}