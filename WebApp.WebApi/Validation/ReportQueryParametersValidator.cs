using FluentValidation;
using WebApp.Infrastructure.Models.ReportModels;

namespace WebApp.WebApi.Validation;

public class ReportQueryParametersValidator : AbstractValidator<ReportQueryParametersModel>
{
    public ReportQueryParametersValidator()
    {
        this.RuleFor(x => x.Page)
            .GreaterThan(0)
            .WithMessage("Page number must be greater than 0.");

        this.RuleFor(x => x.Size)
            .InclusiveBetween(1, 100)
            .WithMessage("Page size must be between 1 and 100.");

        this.RuleFor(x => x.Search)
            .MaximumLength(50)
            .WithMessage("Search term cannot exceed 50 characters.");

        this.RuleFor(x => x.SortBy)
            .NotEmpty()
            .WithMessage("SortBy field cannot be empty.")
            .Must(field => new[] { "Reason", "Date", "User" }.Contains(field))
            .WithMessage("SortBy must be one of the following: Reason, Date, Status.");
    }
}
