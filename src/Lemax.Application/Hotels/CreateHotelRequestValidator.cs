using FluentValidation;
using Lemax.Application.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lemax.Application.Hotels;

public class CreateHotelRequestValidator : CustomValidator<CreateHotelRequest>
{
    public CreateHotelRequestValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty();
        RuleFor(x => x.Price)
            .GreaterThan(0);
    }
}

