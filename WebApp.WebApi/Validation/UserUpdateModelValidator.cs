using FluentValidation;
using WebApp.Infrastructure.Enums;
using WebApp.Infrastructure.Models.UserModels;

namespace WebApp.WebApi.Validation;

public class UserUpdateModelValidator : AbstractValidator<UserUpdateModel>
{
    public UserUpdateModelValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0)
            .WithMessage("User ID must be a positive integer.");

        RuleFor(x => x)
            .Must(x => !string.IsNullOrEmpty(x.Nickname) || !string.IsNullOrEmpty(x.Email))
            .WithMessage("At least one field other than ID must be provided.");

        RuleFor(x => x.Nickname)
            .MinimumLength(1)
            .When(x => !string.IsNullOrEmpty(x.Nickname))
            .WithMessage("Nickname must be non-empty if provided.");

        RuleFor(x => x.Email)
            .EmailAddress()
            .When(x => !string.IsNullOrEmpty(x.Email))
            .WithMessage("Email must be a valid email address if provided.");
    }
}
