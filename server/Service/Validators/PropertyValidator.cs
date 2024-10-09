using FluentValidation;
using Service.TransferModels.Responses;

namespace Service.Validators;

public class PropertyValidator : AbstractValidator<PropertyDto>
{
    public PropertyValidator()
    {
        RuleFor(p => p.PropertyName.Length).GreaterThanOrEqualTo(2);
    }
}