using FluentValidation;
using WebApp.Infrastructure.Models.MessageModels;

namespace WebApp.WebApi.Validation;

public class MessageUpdateModelValidator : AbstractValidator<MessageUpdateModel>
{
    public MessageUpdateModelValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0)
            .WithMessage("Message ID must be a positive integer.");

        RuleFor(x => x.Content)
            .MinimumLength(1)
            .When(x => !string.IsNullOrEmpty(x.Content))
            .WithMessage("Content must be non-empty if provided.");
    }
}
