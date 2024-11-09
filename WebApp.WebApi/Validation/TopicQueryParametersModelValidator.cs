using FluentValidation;
using WebApp.Infrastructure.Models.TopicModels;

namespace WebApp.WebApi.Validation;

public class TopicQueryParametersValidator : AbstractValidator<TopicQueryParametersModel>
{
    public TopicQueryParametersValidator()
    {
        RuleFor(x => x.Page)
            .GreaterThan(0)
            .WithMessage("Page number must be greater than 0.");

        RuleFor(x => x.Size)
            .InclusiveBetween(1, 100)
            .WithMessage("Page size must be between 1 and 100.");

        RuleFor(x => x.Search)
            .MaximumLength(50)
            .WithMessage("Search term cannot exceed 50 characters.");

        RuleFor(x => x.SortBy)
            .NotEmpty()
            .WithMessage("SortBy field cannot be empty.")
            .Must(field => new[] { "Title", "Date", "Author" }.Contains(field))
            .WithMessage("SortBy must be one of the following: Title, CreationDate, Author.");
    }
}
