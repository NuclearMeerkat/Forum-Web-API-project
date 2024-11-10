﻿using FluentValidation;
using WebApp.Infrastructure.Entities;

namespace WebApp.WebApi.Validation;

public class CompositeKeyValidator : AbstractValidator<CompositeKey>
{
    public CompositeKeyValidator()
    {
        RuleFor(x => x.KeyPart1)
            .GreaterThan(0)
            .WithMessage("User ID must be a positive integer.");

        RuleFor(x => x.KeyPart2)
            .GreaterThan(0)
            .WithMessage("Message ID must be a positive integer.");
    }
}