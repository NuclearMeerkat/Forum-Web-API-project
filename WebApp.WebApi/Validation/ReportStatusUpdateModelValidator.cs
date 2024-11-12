using FluentValidation;
using WebApp.Infrastructure.Models.ReportModels;

namespace WebApp.WebApi.Validation;

public class ReportStatusUpdateModelValidator : AbstractValidator<ReportStatusUpdateModel>
{
    public ReportStatusUpdateModelValidator()
    {
        this.RuleFor(x => x.MessageId)
            .GreaterThan(0)
            .WithMessage("Message ID must be a positive integer.");

        this.RuleFor(x => x.Status)
            .IsInEnum()
            .WithMessage("Invalid status value provided.");
    }
}
