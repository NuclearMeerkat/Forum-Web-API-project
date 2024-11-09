using FluentValidation;
using WebApp.Infrastructure.Models.TopicModels;

namespace WebApp.WebApi.Validation;

public class TopicUpdateModelValidator : AbstractValidator<TopicUpdateModel>
{
    public TopicUpdateModelValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0)
            .WithMessage("Topic ID must be a positive integer.");

        RuleFor(x => x)
            .Must(x => !string.IsNullOrEmpty(x.Title) || !string.IsNullOrEmpty(x.Description))
            .WithMessage("At least one field other than ID must be provided.");

        RuleFor(x => x.Title)
            .MinimumLength(1)
            .When(x => !string.IsNullOrWhiteSpace(x.Title))
            .WithMessage("Title must be non-empty if provided.");

        RuleFor(x => x.Description)
            .MinimumLength(1)
            .When(x => !string.IsNullOrWhiteSpace(x.Description))
            .WithMessage("Description must be non-empty if provided.");
    }
}
