using FluentValidation;

namespace WebApp.WebApi.Validation;

public class IdValidator : AbstractValidator<int>
{
    public IdValidator()
    {
        this.RuleFor(x => x)
            .GreaterThan(0)
            .WithMessage("ID must be a positive integer.");
    }
}
