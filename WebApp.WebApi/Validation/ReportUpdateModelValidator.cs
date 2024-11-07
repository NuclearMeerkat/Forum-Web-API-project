using FluentValidation;
using WebApp.Core.Models.ReportModels;

namespace WebApp.WebApi.Validation;

public class ReportUpdateModelValidator : AbstractValidator<ReportUpdateModel>
{
    public ReportUpdateModelValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0)
            .WithMessage("Report ID must be a positive integer.");

        RuleFor(x => x.Reason)
            .MinimumLength(1)
            .When(x => !string.IsNullOrEmpty(x.Reason))
            .WithMessage("Reason must be non-empty if provided.");
    }
}
