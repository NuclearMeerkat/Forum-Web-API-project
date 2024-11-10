using FluentValidation;
using WebApp.Infrastructure.Models.TopicModels;

namespace WebApp.WebApi.Validation;

public class DeleteRateModelValidator : AbstractValidator<DeleteRateModel>
{
    public DeleteRateModelValidator()
    {
        RuleFor(x => x.UserId)
            .GreaterThan(0)
            .WithMessage("User ID must be a positive integer.");

        RuleFor(x => x.TopicId)
            .GreaterThan(0)
            .WithMessage("Topic ID must be a positive integer.");
    }
}
