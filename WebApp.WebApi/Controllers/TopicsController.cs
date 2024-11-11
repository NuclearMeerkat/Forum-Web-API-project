using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApp.BusinessLogic.Validation;
using WebApp.Infrastructure.Interfaces.IServices;
using WebApp.Infrastructure.Models.TopicModels;

namespace WebApp.WebApi.Controllers;

/// <summary>
/// Controller for managing forum topics, including creating, retrieving, updating, and rating topics.
/// </summary>
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

    /// <summary>
    /// Retrieves a list of topics based on specified query parameters.
    /// </summary>
    /// <param name="parameters">Query parameters for filtering, sorting and pagination.</param>
    /// <returns>A list of topics matching the criteria.</returns>
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

    /// <summary>
    /// Retrieves a single topic with its associated dialog by topic ID.
    /// </summary>
    /// <param name="topicId">The ID of the topic to retrieve.</param>
    /// <returns>The topic with detailed information, if found.</returns>
    [HttpGet("{topicId}")]
    public async Task<IActionResult> GetTopicWithDialog(int topicId)
    {
        TopicDialogModel topic;
        try
        {
            topic = await this.topicService.GetByIdWithDetailsAsync(topicId);
        }
        catch (ForumException e)
        {
            return this.NotFound(e.Message);
        }
        catch (InvalidOperationException e)
        {
            return this.BadRequest(e.Message);
        }

        return this.Ok(topic);
    }

    /// <summary>
    /// Creates a new topic based on the provided model.
    /// </summary>
    /// <param name="creationModel">The model containing details for the new topic.</param>
    /// <returns>The created topic, if successful.</returns>
    [Authorize]
    [HttpPost]
    public async Task<IActionResult> CreateTopic(
        [FromBody] TopicCreateModel creationModel)
    {
        var validator = this.serviceProvider.GetService<IValidator<TopicCreateModel>>();

        creationModel.UserId = this.GetCurrentUserId(this.httpContextAccessor);

        return await this.ValidateAndExecuteAsync(creationModel, validator, async () =>
        {
            try
            {
                var topic = this.serviceProvider.GetService<IMapper>().Map<AdminTopicCreateModel>(creationModel);
                int id = await this.topicService.AddAsync(topic);
                creationModel.Id = id;
                return this.CreatedAtAction(nameof(this.CreateTopic), creationModel);
            }
            catch (ForumException e)
            {
                return this.BadRequest(e.Message);
            }
        });
    }

    /// <summary>
    /// Creates a new topic on behalf of a user (Admin/Moderator only).
    /// </summary>
    /// <param name="creationModel">The model with details for the new topic.</param>
    /// <returns>The created topic, if successful.</returns>
    // [Authorize(Roles = "Admin,Moderator")]
    [HttpPost("admin")]
    public async Task<IActionResult> CreateTopicForUser(
        [FromBody] AdminTopicCreateModel creationModel)
    {
        var validator = this.serviceProvider.GetService<IValidator<AdminTopicCreateModel>>();

        return await this.ValidateAndExecuteAsync(creationModel, validator, async () =>
        {
            try
            {
                int id = await this.topicService.AddAsync(creationModel);
                creationModel.Id = id;
                return this.CreatedAtAction(nameof(this.CreateTopic), creationModel);
            }
            catch (ForumException e)
            {
                return this.BadRequest(e.Message);
            }
        });
    }

    /// <summary>
    /// Updates a topic by ID for an admin user.
    /// </summary>
    /// <param name="id">The ID of the topic to update.</param>
    /// <param name="updateModel">The updated data for the topic.</param>
    /// <returns>The updated topic if successful, otherwise an error message.</returns>
    // [Authorize(Roles = "Admin,Moderator")]
    [HttpPatch("admin/{id:int}")]
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

    /// <summary>
    /// Updates a topic by ID for the current user.
    /// </summary>
    /// <param name="id">The ID of the topic to update.</param>
    /// <param name="updateModel">The updated data for the topic.</param>
    /// <returns>The updated topic if successful, otherwise an error message.</returns>
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

    /// <summary>
    /// Deletes a topic by ID (Admin/Moderator only).
    /// </summary>
    /// <param name="id">The ID of the topic to delete.</param>
    /// <returns>No content if deletion is successful.</returns>
    // [Authorize(Roles = "Admin,Moderator")]
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteTopic(int id)
    {
        try
        {
            await this.topicService.DeleteAsync(id);
            return this.NoContent();
        }
        catch (ForumException e)
        {
            return this.NotFound(e.Message);
        }
        catch (InvalidOperationException e)
        {
            return this.BadRequest(e.Message);
        }
    }

    /// <summary>
    /// Rates a topic by providing a rating model. If rating was already rated so updating it.
    /// </summary>
    /// <param name="model">The rating model for a topic.</param>
    /// <returns>Success status if rating is successfully added.</returns>
    [HttpPost("rate")]
    [Authorize]
    public async Task<IActionResult> RateTopic([FromBody] RateTopicModel model)
    {
        var validator = this.serviceProvider.GetService<IValidator<RateTopicModel>>();

        model.UserId = this.GetCurrentUserId(this.httpContextAccessor);

        return await this.ValidateAndExecuteAsync(model, validator, async () =>
        {
            try
            {
                await this.topicService.RateTopic(model.UserId, model.TopicId, model.Stars);
                return this.Ok();
            }
            catch (ForumException e)
            {
                return this.NotFound(e.Message);
            }
        });
    }

    /// <summary>
    /// Removes a rating from a topic for the current user.
    /// </summary>
    /// <param name="topicId">The ID of the topic for which to remove the rating.</param>
    /// <returns>Success status if rating is removed successfully.</returns>
    [HttpDelete("{topicId:int}/rate")]
    [Authorize]
    public async Task<IActionResult> DeleteRateTopic(int topicId)
    {
        DeleteRateModel model = new DeleteRateModel()
        {
            UserId = this.GetCurrentUserId(this.httpContextAccessor),
            TopicId = topicId,
        };

        var validator = this.serviceProvider.GetService<IValidator<DeleteRateModel>>();

        return await this.ValidateAndExecuteAsync(model, validator, async () =>
        {
            try
            {
                if (!await this.topicService.CheckTopicOwner(model.TopicId, model.UserId))
                {
                    throw new ForumException("You cannot delete this rate");
                }

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
