using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApp.BusinessLogic.Validation;
using WebApp.WebApi.Validation;

namespace WebApp.WebApi.Utilities;

public class RequestProcessor
{
    private readonly ModelValidator validator;

    public RequestProcessor(ModelValidator validator)
    {
        this.validator = validator;
    }

    public async Task<IActionResult> ProcessRequestAsync<TModel>(
        TModel model,
        Func<Task<IActionResult>> action)
    {
        var validationResult = await this.validator.ValidateAsync(model);
        if (!validationResult.IsValid)
        {
            return new BadRequestObjectResult(validationResult.Errors);
        }

        try
        {
            ArgumentNullException.ThrowIfNull(action);
            return await action();
        }
        catch (ForumException e)
        {
            return new NotFoundObjectResult(e.Message);
        }
        catch (InvalidOperationException e)
        {
            return new BadRequestObjectResult(e.Message);
        }
        catch (UnauthorizedAccessException e)
        {
            return new UnauthorizedObjectResult(e.Message);
        }
        catch (DbUpdateException e)
        {
            return new ConflictObjectResult(e.Message);
        }
    }
}
