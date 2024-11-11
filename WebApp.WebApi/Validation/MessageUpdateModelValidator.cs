using FluentValidation;
using WebApp.Infrastructure.Models.MessageModels;

namespace WebApp.WebApi.Validation;

public class MessageUpdateModelValidator : AbstractValidator<MessageUpdateModel>
{
    public MessageUpdateModelValidator()
    {
        this.RuleFor(x => x.Id)
            .GreaterThan(0)
            .WithMessage("Message ID must be a positive integer.");

        this.RuleFor(x => x.Content)
            .MinimumLength(1)
            .When(x => !string.IsNullOrEmpty(x.Content))
            .WithMessage("Content must be non-empty if provided.");
    }
}
