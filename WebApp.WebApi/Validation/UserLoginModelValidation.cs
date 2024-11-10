using System.Data;
using FluentValidation;
using WebApp.Infrastructure.Models.UserModels;

namespace WebApp.WebApi.Validation;

public class UserLoginModelValidation : AbstractValidator<UserLoginModel>
{
    public UserLoginModelValidation()
    {
        RuleFor(x => x.Email)
            .EmailAddress()
            .NotEmpty()
            .WithMessage("Email cannot be empty string");

        RuleFor(x => x.Password)
            .NotEmpty()
            .WithMessage("Password cannot be empty string");
    }
}
