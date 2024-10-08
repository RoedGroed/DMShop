﻿using FluentValidation;
using Service.TransferModels.Responses;

namespace Service.Validators;

public class CreatePaperValidator : AbstractValidator<ProductDto>
{

    public CreatePaperValidator()
    {
        RuleFor(p => p.Name.Length).GreaterThanOrEqualTo(2);
        RuleFor(p => p.Price).NotEmpty().GreaterThan(0);
        // implement. Need frontend message of why it failed, also in paper. 
        // add rules for price
    }
}