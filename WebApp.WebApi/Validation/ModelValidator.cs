using FluentValidation;
using FluentValidation.Results;

namespace WebApp.WebApi.Validation;

public class ModelValidator
{
    private readonly IServiceProvider serviceProvider;

    public ModelValidator(IServiceProvider serviceProvider)
    {
        this.serviceProvider = serviceProvider;
    }

    public async Task<ValidationResult> ValidateAsync<TModel>(TModel model)
    {
        var validator = this.serviceProvider.GetService<IValidator<TModel>>();
        if (validator == null)
        {
            throw new InvalidOperationException($"Validator for {typeof(TModel).Name} not found.");
        }

        return await validator.ValidateAsync(model);
    }
}
