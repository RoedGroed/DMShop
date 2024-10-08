using FluentValidation;
using Service.TransferModels.Responses;

namespace Service.Validators;

public class PropertyValidator : AbstractValidator<PropertyDto>
{
    public PropertyValidator()
    {
        // implement. Need frontend message of why it failed, also in paper. 
    }
}