using AutoMapper;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using WebApp.BusinessLogic.Validation;
using WebApp.Core.Interfaces.IServices;
using WebApp.Core.Models.TopicModels;

namespace WebApp.WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TopicsController : ControllerBase
{
    private readonly ITopicService topicService;
    private readonly IValidator<TopicCreateModel> topicCreateValidator;
    private readonly IValidator<TopicUpdateModel> topicUpdateValidator;

    public TopicsController(
        ITopicService topicService,
        IValidator<TopicCreateModel> topicCreateValidator,
        IValidator<TopicUpdateModel> topicUpdateValidator)
    {
        this.topicService = topicService;
        this.topicCreateValidator = topicCreateValidator;
        this.topicUpdateValidator = topicUpdateValidator;
    }

    // GET: api/topics
    [HttpGet]
    public async Task<IActionResult> Get([FromQuery] TopicQueryParametersModel parameters)
    {
        var topics = await this.topicService.GetAllAsync(parameters);
        return this.Ok(topics);
    }

    // GET: api/topics/{id}
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        TopicSummaryModel topic;
        try
        {
            topic = await this.topicService.GetByIdAsync(id);
        }
        catch (ForumException)
        {
            return this.NotFound();
        }

        return this.Ok(topic);
    }

    // POST: api/topics
    // [Authorize]
    [HttpPost]
    public async Task<IActionResult> Post(
        [FromBody] TopicCreateModel creationModel,
        [FromServices] IValidator<TopicCreateModel> validator)
    {
        ValidationResult result = await this.topicCreateValidator.ValidateAsync(creationModel);

        if (!result.IsValid)
        {
            var modelStateDict = new ModelStateDictionary();

            foreach (var error in result.Errors)
            {
                modelStateDict.AddModelError(error.PropertyName, error.ErrorMessage);
            }

            return this.ValidationProblem(modelStateDict);
        }

        try
        {
            int id = await this.topicService.AddAsync(creationModel);

            return this.CreatedAtAction(nameof(this.GetById), id, creationModel);
        }
        catch (ForumException e)
        {
            return this.BadRequest(e.Message);
        }
        catch (Exception)
        {
            Console.WriteLine("Unexpected exception occured in CreateTopic");
            throw;
        }
    }

    // PUT: api/topics/{id}
    // [Authorize]
    [HttpPut("{id:int}")]
    public async Task<IActionResult> Put(
        int id,
        [FromBody] TopicUpdateModel updateModel,
        [FromServices] IValidator<TopicUpdateModel> validator)
    {
        updateModel.Id = id;

        ValidationResult result = await this.topicUpdateValidator.ValidateAsync(updateModel);

        if (!result.IsValid)
        {
            var modelStateDict = new ModelStateDictionary();

            foreach (var error in result.Errors)
            {
                modelStateDict.AddModelError(error.PropertyName, error.ErrorMessage);
            }

            return this.ValidationProblem(modelStateDict);
        }

        try
        {
            await this.topicService.UpdateAsync(updateModel);
            return this.NoContent();
        }
        catch (ForumException)
        {
            return this.BadRequest();
        }
    }

    // DELETE: api/topics/{id}
    // [Authorize(Roles = "Admin,Moderator")]
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteTopic(int id)
    {
        await this.topicService.DeleteAsync(id);
        return this.NoContent();
    }
}
