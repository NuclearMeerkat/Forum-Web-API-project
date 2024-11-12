using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApp.Infrastructure.Interfaces.IServices;
using WebApp.Infrastructure.Models.TopicModels;
using WebApp.WebApi.Utilities;

namespace WebApp.WebApi.Controllers;

/// <summary>
/// Controller for managing forum topics, including creating, retrieving, updating, and rating topics.
/// </summary>
[Route("api/[controller]")]
[ApiController]
public class TopicsController : BaseController
{
    private readonly ITopicService topicService;
    private readonly IHttpContextAccessor httpContextAccessor;
    private readonly RequestProcessor requestProcessor;

    public TopicsController(
        ITopicService topicService,
        IHttpContextAccessor httpContextAccessor,
        RequestProcessor requestProcessor)
    {
        ArgumentNullException.ThrowIfNull(topicService);
        ArgumentNullException.ThrowIfNull(httpContextAccessor);
        ArgumentNullException.ThrowIfNull(requestProcessor);
        this.topicService = topicService;
        this.httpContextAccessor = httpContextAccessor;
        this.requestProcessor = requestProcessor;
    }

    /// <summary>
    /// Retrieves a list of topics based on specified query parameters.
    /// </summary>
    /// <param name="parameters">Query parameters for filtering, sorting and pagination.</param>
    /// <returns>A list of topics matching the criteria.</returns>
    [HttpGet]
    public async Task<IActionResult> GetTopics([FromQuery] TopicQueryParametersModel parameters)
    {
        return await this.requestProcessor.ProcessRequestAsync(parameters, async () =>
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
        return await this.requestProcessor.ProcessRequestAsync(topicId, async () =>
        {
            var topic = await this.topicService.GetByIdWithDetailsAsync(topicId);
            return this.Ok(topic);
        });
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
        creationModel.UserId = GetCurrentUserId(this.httpContextAccessor);
        return await this.requestProcessor.ProcessRequestAsync(creationModel, async () =>
        {
            var topic = new AdminTopicCreateModel()
            {
                Id = creationModel.Id,
                CreatedAt = creationModel.CreatedAt,
                Description = creationModel.Description,
                Title = creationModel.Title,
                UserId = creationModel.UserId,
            };
            int id = await this.topicService.AddAsync(topic);
            creationModel.Id = id;
            return this.CreatedAtAction(nameof(this.CreateTopic), creationModel);
        });
    }

    /// <summary>
    /// Creates a new topic on behalf of a user (Admin/Moderator only).
    /// </summary>
    /// <param name="creationModel">The model with details for the new topic.</param>
    /// <returns>The created topic, if successful.</returns>
    [Authorize(Policy = "ModeratorAccess")]
    [HttpPost("admin")]
    public async Task<IActionResult> CreateTopicForUser(
        [FromBody] AdminTopicCreateModel creationModel)
    {
        return await this.requestProcessor.ProcessRequestAsync(creationModel, async () =>
        {
            int id = await this.topicService.AddAsync(creationModel);
            creationModel.Id = id;
            return this.CreatedAtAction(nameof(this.CreateTopic), creationModel);
        });
    }

    /// <summary>
    /// Updates a topic by ID for an admin user.
    /// </summary>
    /// <param name="id">The ID of the topic to update.</param>
    /// <param name="updateModel">The updated data for the topic.</param>
    /// <returns>The updated topic if successful, otherwise an error message.</returns>
    [HttpPatch("admin/{id:int}")]
    [Authorize(Policy = "ModeratorAccess")]
    public async Task<IActionResult> AdminUpdateTopic(
        int id,
        [FromBody] TopicUpdateModel updateModel)
    {
        updateModel.Id = id;
        return await this.requestProcessor.ProcessRequestAsync(updateModel, async () =>
        {
            await this.topicService.UpdateAsync(updateModel);
            var updatedTopic = await this.topicService.GetByIdAsync(updateModel.Id);
            return this.Ok(updatedTopic);
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
        return await this.requestProcessor.ProcessRequestAsync(updateModel, async () =>
        {
            int userId = GetCurrentUserId(this.httpContextAccessor);
            await this.topicService.UpdateAsync(updateModel, userId);
            var updatedTopic = await this.topicService.GetByIdAsync(updateModel.Id);
            return this.Ok(updatedTopic);
        });
    }

    /// <summary>
    /// Deletes a topic by ID (Admin/Moderator only).
    /// </summary>
    /// <param name="id">The ID of the topic to delete.</param>
    /// <returns>No content if deletion is successful.</returns>
    [HttpDelete("admin/{id:int}")]
    [Authorize(Policy = "AdminOnly")]
    public async Task<IActionResult> AdminDeleteTopic(int id)
    {
        return await this.requestProcessor.ProcessRequestAsync(id, async () =>
        {
            await this.topicService.DeleteAsync(id);
            return this.NoContent();
        });
    }

    /// <summary>
    /// Deletes a user topic by ID.
    /// </summary>
    /// <param name="id">The ID of your topic to delete.</param>
    /// <returns>No content if deletion is successful.</returns>
    [HttpDelete("{id:int}")]
    [Authorize]
    public async Task<IActionResult> DeleteTopic(int id)
    {
        int userId = GetCurrentUserId(this.httpContextAccessor);
        return await this.requestProcessor.ProcessRequestAsync(id, async () =>
        {
            await this.topicService.DeleteAsync(id, userId);
            return this.NoContent();
        });
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
        return await this.requestProcessor.ProcessRequestAsync(model, async () =>
        {
            model.UserId = GetCurrentUserId(this.httpContextAccessor);
            await this.topicService.RateTopicAsync(model.UserId, model.TopicId, model.Stars);
            return this.Ok();
        });
    }

    /// <summary>
    /// Removes a rating from a topic (Only for Admin/Moderator).
    /// </summary>
    /// <param name="topicId">The ID of the topic for which to remove the rating.</param>
    /// <returns>Success status if rating is removed successfully.</returns>
    [HttpDelete("admin/{topicId:int}/rate")]
    [Authorize(Policy = "ModeratorAccess")]
    public async Task<IActionResult> AdminDeleteRateTopic(int topicId)
    {
        DeleteRateModel model = new DeleteRateModel()
        {
            UserId = GetCurrentUserId(this.httpContextAccessor),
            TopicId = topicId,
        };
        return await this.requestProcessor.ProcessRequestAsync(model, async () =>
        {
            await this.topicService.RemoveRateAsync(model.UserId, model.TopicId);
            return this.Ok();
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
            UserId = GetCurrentUserId(this.httpContextAccessor),
            TopicId = topicId,
        };
        return await this.requestProcessor.ProcessRequestAsync(model, async () =>
        {
            await this.topicService.RemoveRateWithUserOwnerCheck(model.UserId, model.TopicId);
            return this.Ok();
        });
    }
}
