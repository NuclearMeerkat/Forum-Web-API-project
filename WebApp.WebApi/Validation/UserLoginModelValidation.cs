using System.Data;
using FluentValidation;
using WebApp.Infrastructure.Models.UserModels;

namespace WebApp.WebApi.Validation;

public class UserLoginModelValidation : AbstractValidator<UserLoginModel>
{
    public UserLoginModelValidation()
    {
        RuleFor(x => x.Email)
            .MinimumLength(6)
            .WithMessage("Email must be at least 6 characters long");

        RuleFor(x => x.Password)
            .MinimumLength(8)
            .WithMessage("Password must be at least 8 characters long");
    }
}
