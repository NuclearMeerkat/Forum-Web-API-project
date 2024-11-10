using FluentValidation;
using WebApp.Infrastructure.Models.TopicModels;

namespace WebApp.WebApi.Validation;

public class AdminTopicCreateModelValidator : AbstractValidator<AdminTopicCreateModel>
{
    public AdminTopicCreateModelValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty()
            .WithMessage("Title is required.");

        RuleFor(x => x.Description)
            .NotEmpty()
            .WithMessage("Description is required.");

        RuleFor(x => x.UserId)
            .GreaterThan(0)
            .WithMessage("UserId must be a positive integer.");
    }
}
