using FluentValidation;
using WebApp.Core.Models.ReportModels;

namespace WebApp.WebApi.Validation;

public class ReportCreateModelValidator : AbstractValidator<ReportCreateModel>
{
    public ReportCreateModelValidator()
    {
        RuleFor(x => x.UserId)
            .GreaterThan(0)
            .WithMessage("UserId must be a positive integer.");

        RuleFor(x => x.MessageId)
            .GreaterThan(0)
            .WithMessage("MessageId must be a positive integer.");

        RuleFor(x => x.Reason)
            .NotEmpty()
            .WithMessage("Reason is required.");
    }
}
