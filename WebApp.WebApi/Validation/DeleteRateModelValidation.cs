using FluentValidation;
using WebApp.Infrastructure.Models.TopicModels;

namespace WebApp.WebApi.Validation;

public class DeleteRateModelValidation : AbstractValidator<DeleteRateModel>
{
    public DeleteRateModelValidation()
    {
        RuleFor(x => x.UserId)
            .GreaterThan(0)
            .WithMessage("User ID must be a positive integer.");

        RuleFor(x => x.TopicId)
            .GreaterThan(0)
            .WithMessage("Topic ID must be a positive integer.");
    }
}
