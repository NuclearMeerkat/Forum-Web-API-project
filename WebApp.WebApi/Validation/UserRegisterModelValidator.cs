using FluentValidation;
using WebApp.Infrastructure.Models.UserModels;

namespace WebApp.WebApi.Validation;

public class UserRegisterModelValidator : AbstractValidator<UserRegisterModel>
{
    public UserRegisterModelValidator()
    {
        RuleFor(x => x.Nickname)
            .NotEmpty()
            .WithMessage("Nickname is required.");

        RuleFor(x => x.Email)
            .NotEmpty()
            .WithMessage("Email is required.");

        RuleFor(x => x.Role)
            .IsInEnum()
            .WithMessage("Role must be a valid enum value.");
    }
}
