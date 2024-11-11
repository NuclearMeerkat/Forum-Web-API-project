using FluentValidation;
using WebApp.Infrastructure.Models.UserModels;

namespace WebApp.WebApi.Validation;

public class UserQueryParametersModelValidator : AbstractValidator<UserQueryParametersModel>
{
    public UserQueryParametersModelValidator()
    {
        this.RuleFor(x => x.Page)
            .GreaterThanOrEqualTo(1).WithMessage("Page number must be at least 1.");

        this.RuleFor(x => x.Size)
            .InclusiveBetween(1, 100).WithMessage("Size must be between 1 and 100.");

        this.RuleFor(x => x.SortBy)
            .Must(value => new[] { "Nickname", "CreatedDate" }.Contains(value))
            .WithMessage("SortBy must be one of 'Username' or 'CreatedDate'.");

        this.RuleFor(x => x.Search)
            .MaximumLength(50).WithMessage("Search term must not exceed 50 characters.");
    }
}
