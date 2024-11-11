using FluentValidation;
using WebApp.Infrastructure.Models.TopicModels;

namespace WebApp.WebApi.Validation;

public class AdminTopicCreateModelValidator : AbstractValidator<AdminTopicCreateModel>
{
    public AdminTopicCreateModelValidator()
    {
        this.RuleFor(x => x.Title)
            .NotEmpty()
            .WithMessage("Title is required.");

        this.RuleFor(x => x.Description)
            .NotEmpty()
            .WithMessage("Description is required.");

        this.RuleFor(x => x.UserId)
            .GreaterThan(0)
            .WithMessage("UserId must be a positive integer.");
    }
}
