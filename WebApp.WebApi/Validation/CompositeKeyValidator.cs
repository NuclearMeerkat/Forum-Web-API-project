using FluentValidation;
using WebApp.Infrastructure.Entities;

namespace WebApp.WebApi.Validation;

public class CompositeKeyValidator : AbstractValidator<ReportCompositeKey>
{
    public CompositeKeyValidator()
    {
        this.RuleFor(x => x.UserId)
            .GreaterThan(0)
            .WithMessage("User ID must be a positive integer.");

        this.RuleFor(x => x.MessageId)
            .GreaterThan(0)
            .WithMessage("Message ID must be a positive integer.");
    }
}
