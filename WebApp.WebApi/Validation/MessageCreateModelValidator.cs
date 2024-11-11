using FluentValidation;
using WebApp.Infrastructure.Models.MessageModels;

namespace WebApp.WebApi.Validation;

public class MessageCreateModelValidator : AbstractValidator<MessageCreateModel>
{
    public MessageCreateModelValidator()
    {
        this.RuleFor(x => x.UserId)
            .GreaterThan(0)
            .WithMessage("UserId must be a positive integer.");

        this.RuleFor(x => x.TopicId)
            .GreaterThan(0)
            .WithMessage("TopicId must be a positive integer.");

        this.RuleFor(x => x.Content)
            .NotEmpty()
            .WithMessage("Content is required.");

        this.RuleFor(x => x.ParentMessageId)
            .GreaterThan(0)
            .When(x => x.ParentMessageId != null);
    }
}
