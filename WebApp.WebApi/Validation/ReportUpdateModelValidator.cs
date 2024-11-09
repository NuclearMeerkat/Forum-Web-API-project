﻿using FluentValidation;
using WebApp.Infrastructure.Enums;
using WebApp.Infrastructure.Models.ReportModels;

namespace WebApp.WebApi.Validation;

public class ReportUpdateModelValidator : AbstractValidator<ReportUpdateModel>
{
    public ReportUpdateModelValidator()
    {
        RuleFor(x => x.UserId)
            .GreaterThan(0)
            .WithMessage("User ID must be a positive integer.");

        RuleFor(x => x.MessageId)
            .GreaterThan(0)
            .WithMessage("Message ID must be a positive integer.");

        RuleFor(x => x)
            .Must(x => !string.IsNullOrEmpty(x.Reason) || x.Status != null || x.ReviewedAt != null)
            .WithMessage("At least one field other than ID must be provided.");

        RuleFor(x => x.Reason)
            .MinimumLength(1)
            .When(x => !string.IsNullOrEmpty(x.Reason))
            .WithMessage("Reason must be non-empty if provided.");

        RuleFor(x => x.Status)
            .IsInEnum()
            .WithMessage("This status is invalid.");
    }
}
