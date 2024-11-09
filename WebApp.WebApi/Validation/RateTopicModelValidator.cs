using FluentValidation;
using WebApp.Infrastructure.Models.TopicModels;

namespace WebApp.WebApi.Validation;

public class RateTopicModelValidator : AbstractValidator<RateTopicModel>
{
    public RateTopicModelValidator()
    {
        RuleFor(x => x.UserId).GreaterThan(0)
            .WithMessage("UserId ID must be a positive integer.");

        RuleFor(x => x.TopicId).GreaterThan(0)
            .WithMessage("Topic ID must be a positive integer.");

        RuleFor(x => x.Stars).GreaterThanOrEqualTo(0).LessThanOrEqualTo(5)
            .WithMessage("Stars must be in range between 0 and 5.");
    }
}
