using FluentValidation;
using Service.TransferModels.Responses;

namespace Service.Validators;

public class CreatePaperValidator : AbstractValidator<ProductDto>
{

    public CreatePaperValidator()
    {
        RuleFor(p => p.Name.Length).GreaterThanOrEqualTo(2).WithMessage("Name must be at least 2 or more characters");
        RuleFor(p => p.Price).NotEmpty().GreaterThan(0).WithMessage("Price must not be empty");
    }
}