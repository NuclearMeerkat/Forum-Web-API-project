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
    private readonly IHttpContextAccessor httpContextAccessor;

    public TopicsController(
        ITopicService topicService,
        IServiceProvider serviceProvider,
        IHttpContextAccessor httpContextAccessor)
    {
        ArgumentNullException.ThrowIfNull(topicService);
        ArgumentNullException.ThrowIfNull(serviceProvider);
        ArgumentNullException.ThrowIfNull(httpContextAccessor);
        this.topicService = topicService;
        this.serviceProvider = serviceProvider;
        this.httpContextAccessor = httpContextAccessor;
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
    [Authorize]
    [HttpPost]
    public async Task<IActionResult> CreateTopic(
        [FromBody] TopicCreateModel creationModel)
    {
        var validator = this.serviceProvider.GetService<IValidator<TopicCreateModel>>();

        creationModel.UserId = this.GetCurrentUserId(httpContextAccessor);

        return await this.ValidateAndExecuteAsync(creationModel, validator, async () =>
        {
            try
            {
                var topic = this.serviceProvider.GetService<IMapper>().Map<AdminTopicCreateModel>(creationModel);
                int id = await this.topicService.AddAsync(topic);
                return this.CreatedAtAction(nameof(this.CreateTopic), creationModel);
            }
            catch (ForumException e)
            {
                return this.BadRequest(e.Message);
            }
        });
    }

    // [Authorize(Roles = "Admin,Moderator")]
    [HttpPost("Admin")]
    public async Task<IActionResult> CreateTopicForUser(
        [FromBody] AdminTopicCreateModel creationModel)
    {
        var validator = this.serviceProvider.GetService<IValidator<AdminTopicCreateModel>>();

        return await this.ValidateAndExecuteAsync(creationModel, validator, async () =>
        {
            try
            {
                int id = await this.topicService.AddAsync(creationModel);
                return this.CreatedAtAction(nameof(this.CreateTopic), creationModel);
            }
            catch (ForumException e)
            {
                return this.BadRequest(e.Message);
            }
        });
    }

    // PUT: api/topics/{id}
    // [Authorize(Roles = "Admin,Moderator")]
    [HttpPatch("Admin/{id:int}")]
    public async Task<IActionResult> AdminUpdateTopic(
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
            catch (ForumException e)
            {
                return this.NotFound(e.Message);
            }
        });
    }

    [HttpPatch("{id:int}")]
    [Authorize]
    public async Task<IActionResult> UpdateTopic(
        int id,
        [FromBody] TopicUpdateModel updateModel)
    {
        updateModel.Id = id;

        var validator = this.serviceProvider.GetService<IValidator<TopicUpdateModel>>();

        int userId = this.GetCurrentUserId(this.httpContextAccessor);

        return await this.ValidateAndExecuteAsync(updateModel, validator, async () =>
        {
            try
            {
                if (!await this.topicService.CheckTopicOwner(updateModel.Id, userId))
                {
                    return this.Forbid("You cannot update this topic");
                }

                await this.topicService.UpdateAsync(updateModel);
                var updatedTopic = await this.topicService.GetByIdAsync(updateModel.Id);
                return this.Ok(updatedTopic);
            }
            catch (ForumException e)
            {
                return this.NotFound(e.Message);
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
