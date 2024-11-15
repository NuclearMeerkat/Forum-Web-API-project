using FluentValidation;
using WebApp.Infrastructure.Models.UserModels;

namespace WebApp.WebApi.Validation;

public class UserRegisterModelValidator : AbstractValidator<UserRegisterModel>
{
    public UserRegisterModelValidator()
    {
        this.RuleFor(x => x.Nickname)
            .MinimumLength(3)
            .WithMessage("Nickname should contain at least 3 characters.");

        this.RuleFor(x => x.Email)
            .EmailAddress()
            .MinimumLength(6)
            .WithMessage("Email should contain at least 6 characters.");

        this.RuleFor(x => x.Role)
            .IsInEnum()
            .WithMessage("Role must be a valid enum value.");
    }
}
