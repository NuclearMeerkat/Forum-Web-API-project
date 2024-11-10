using FluentValidation;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace WebApp.WebApi.Controllers;

public class BaseController : ControllerBase
{
    protected async Task<IActionResult> ValidateAndExecuteAsync<TModel>(
        TModel model,
        IValidator<TModel> validator,
        Func<Task<IActionResult>> action)
    {
        var result = await validator.ValidateAsync(model);
        if (!result.IsValid)
        {
            var modelStateDict = new ModelStateDictionary();
            foreach (var error in result.Errors)
            {
                modelStateDict.AddModelError(error.PropertyName, error.ErrorMessage);
            }

            return this.ValidationProblem(modelStateDict);
        }

        return await action();
    }

    protected int GetCurrentUserId(IHttpContextAccessor httpContextAccessor)
    {
        var userId = httpContextAccessor.HttpContext?.User.Claims.First().Value;
        return int.TryParse(userId, out int id) ? id : 0;
    }
}
