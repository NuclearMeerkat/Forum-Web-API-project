using FluentValidation;
using WebApp.Core.Models.UserModels;

namespace WebApp.WebApi.Validation;

public class UserCreateModelValidator : AbstractValidator<UserCreateModel>
{
    public UserCreateModelValidator()
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
