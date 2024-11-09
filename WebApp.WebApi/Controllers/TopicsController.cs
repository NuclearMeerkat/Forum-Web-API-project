using AutoMapper;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using WebApp.BusinessLogic.Validation;
using WebApp.Infrastructure.Entities;
using WebApp.Infrastructure.Interfaces.IServices;
using WebApp.Infrastructure.Models.TopicModels;

namespace WebApp.WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TopicsController : BaseController
{
    private readonly ITopicService topicService;
    private readonly IServiceProvider serviceProvider;

    public TopicsController(ITopicService topicService, IServiceProvider serviceProvider)
    {
        ArgumentNullException.ThrowIfNull(topicService);
        ArgumentNullException.ThrowIfNull(serviceProvider);
        this.topicService = topicService;
        this.serviceProvider = serviceProvider;
    }

    // GET: api/topics
    [HttpGet]
    public async Task<IActionResult> GetTopics([FromQuery] TopicQueryParametersModel parameters)
    {
        var validator = this.serviceProvider.GetService<IValidator<TopicQueryParametersModel>>();

        return await this.ValidateAndExecuteAsync(parameters, validator, async () =>
        {
            var topics = await this.topicService.GetAllAsync(parameters);
            return this.Ok(topics);
        });
    }

    // GET: api/topics/{id}
    [HttpGet("{id}")]
    public async Task<IActionResult> GetTopicById(int id)
    {
        TopicSummaryModel topic;
        try
        {
            topic = await this.topicService.GetByIdAsync(id);
        }
        catch (InvalidOperationException)
        {
            return this.NotFound($"Topic with Id = {id} not found");
        }

        return this.Ok(topic);
    }

    // POST: api/topics
    // [Authorize]
    [HttpPost]
    public async Task<IActionResult> CreateTopic(
        [FromBody] TopicCreateModel creationModel)
    {
        var validator = this.serviceProvider.GetService<IValidator<TopicCreateModel>>();

        return await this.ValidateAndExecuteAsync(creationModel, validator, async () =>
        {
            try
            {
                int id = await this.topicService.RegisterAsync(creationModel);
                return this.CreatedAtAction(nameof(this.CreateTopic), creationModel);
            }
            catch (ForumException e)
            {
                return this.BadRequest(e.Message);
            }
        });
    }

    // PUT: api/topics/{id}
    // [Authorize]
    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateTopic(
        int id,
        [FromBody] TopicUpdateModel updateModel)
    {
        updateModel.Id = id;

        var validator = this.serviceProvider.GetService<IValidator<TopicUpdateModel>>();

        return await this.ValidateAndExecuteAsync(updateModel, validator, async () =>
        {
            try
            {
                await this.topicService.UpdateAsync(updateModel);
                var updatedTopic = await this.topicService.GetByIdAsync(updateModel.Id);
                return this.Ok(updatedTopic);
            }
            catch (ForumException)
            {
                return this.BadRequest();
            }
        });
    }

    // DELETE: api/topics/{id}
    // [Authorize(Roles = "Admin,Moderator")]
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteTopic(int id)
    {
        TopicSummaryModel topic;
        try
        {
            topic = await this.topicService.GetByIdAsync(id);
        }
        catch (InvalidOperationException)
        {
            return this.NotFound($"Topic with Id = {id} not found");
        }

        await this.topicService.DeleteAsync(id);
        return this.NoContent();
    }

    [HttpPost("rate")]
    public async Task<IActionResult> RateTopic([FromBody] RateTopicModel model)
    {
        var validator = this.serviceProvider.GetService<IValidator<RateTopicModel>>();

        return await this.ValidateAndExecuteAsync(model, validator, async () =>
        {
            await this.topicService.RateTopic(model.UserId, model.TopicId, model.Stars);
            return this.Ok();
        });
    }

    [HttpDelete("rate")]
    public async Task<IActionResult> DeleteRateTopic([FromBody] DeleteRateModel model)
    {
        var validator = this.serviceProvider.GetService<IValidator<DeleteRateModel>>();

        return await this.ValidateAndExecuteAsync(model, validator, async () =>
        {
            try
            {
                await this.topicService.RemoveRate(model.UserId, model.TopicId);
                return this.Ok();
            }
            catch (InvalidOperationException ex)
            {
                return this.NotFound(ex.Message);
            }
        });
    }
}
