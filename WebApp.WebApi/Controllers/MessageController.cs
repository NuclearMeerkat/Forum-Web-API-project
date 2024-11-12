using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApp.Infrastructure.Interfaces.IServices;
using WebApp.Infrastructure.Models.MessageModels;
using WebApp.WebApi.Utilities;

namespace WebApp.WebApi.Controllers;

[Route("api")]
[ApiController]
public class MessagesController : BaseController
{
    private readonly IMessageService messageService;
    private readonly IHttpContextAccessor httpContextAccessor;
    private readonly RequestProcessor requestProcessor;

    public MessagesController(
        RequestProcessor requestProcessor,
        IMessageService messageService,
        IServiceProvider serviceProvider,
        IHttpContextAccessor httpContextAccessor)
    {
        ArgumentNullException.ThrowIfNull(messageService);
        ArgumentNullException.ThrowIfNull(serviceProvider);
        ArgumentNullException.ThrowIfNull(httpContextAccessor);
        ArgumentNullException.ThrowIfNull(requestProcessor);
        this.messageService = messageService;
        this.httpContextAccessor = httpContextAccessor;
        this.requestProcessor = requestProcessor;
    }

    /// <summary>
    /// Retrieves a list of messages for a specific topic.
    /// </summary>
    /// <param name="topicId">The ID of the topic to retrieve messages for.</param>
    /// <returns>A list of messages associated with the specified topic.</returns>
    [HttpGet("topics/{topicId}/messages")]
    public async Task<IActionResult> GetMessagesForTopic(int topicId)
    {
        return await this.requestProcessor.ProcessRequestAsync(topicId, async () =>
        {
            var message = await this.messageService.GetMessagesByTopicIdAsync(topicId);
            return this.Ok(message);
        });
    }

    /// <summary>
    /// Retrieves a message by its ID.
    /// </summary>
    /// <param name="id">The ID of the message to retrieve.</param>
    /// <returns>The details of the specified message.</returns>
    [HttpGet("messages/{id}")]
    public async Task<IActionResult> GetMessageById(int id)
    {
        return await this.requestProcessor.ProcessRequestAsync(id, async () =>
        {
            var message = await this.messageService.GetByIdAsync(id);
            return this.Ok(message);
        });
    }

    /// <summary>
    /// Creates a new message under a specific topic.
    /// </summary>
    /// <param name="topicId">The ID of the topic to associate the message with.</param>
    /// <param name="creationModel">The model containing the message details.</param>
    /// <returns>The created message with its details.</returns>
    [Authorize]
    [HttpPost("topics/{topicId}/messages")]
    public async Task<IActionResult> SendMessage(int topicId, [FromBody] MessageCreateModel creationModel)
    {
        ArgumentNullException.ThrowIfNull(creationModel);
        creationModel.TopicId = topicId;
        creationModel.UserId = GetCurrentUserId(this.httpContextAccessor);

        return await this.requestProcessor.ProcessRequestAsync(creationModel, async () =>
        {
            int messageId = await this.messageService.AddAsync(creationModel);
            return this.CreatedAtAction(nameof(this.GetMessageById), new { id = messageId }, creationModel);
        });
    }

    /// <summary>
    /// Partially updates an existing message by its ID.
    /// </summary>
    /// <param name="id">The ID of the message to update.</param>
    /// <param name="updateModel">The model containing updated message details.</param>
    /// <returns>The updated message details.</returns>
    [Authorize]
    [HttpPatch("messages/{id:int}")]
    public async Task<IActionResult> UpdateMessage(int id, [FromBody] MessageUpdateModel updateModel)
    {
        updateModel.Id = id;
        updateModel.UserId = GetCurrentUserId(this.httpContextAccessor);

        return await this.requestProcessor.ProcessRequestAsync(updateModel, async () =>
        {
            await this.messageService.UpdateAsync(updateModel);
            return this.NoContent();
        });
    }

    /// <summary>
    /// Deletes a message by its ID. Admins have permission to delete any message.
    /// </summary>
    /// <param name="id">The ID of the message to delete.</param>
    /// <returns>HTTP status indicating the result of the deletion.</returns>
    [Authorize(Policy = "ModeratorAccess")]
    [HttpDelete("messages/Admin/{id:int}")]
    public async Task<IActionResult> AdminDeleteMessage(int id)
    {
        return await this.requestProcessor.ProcessRequestAsync(id, async () =>
        {
            await this.messageService.DeleteAsync(id);
            return this.NoContent();
        });
    }

    /// <summary>
    /// Deletes the current user's message by its ID.
    /// </summary>
    /// <param name="id">The ID of the message to delete.</param>
    /// <returns>HTTP status indicating the result of the deletion.</returns>
    [HttpDelete("messages/{id:int}")]
    [Authorize]
    public async Task<IActionResult> DeleteMyMessage(int id)
    {
        return await this.requestProcessor.ProcessRequestAsync(id, async () =>
        {
            int userId = GetCurrentUserId(this.httpContextAccessor);
            await this.messageService.DeleteAsync(id, userId);
            return this.NoContent();
        });
    }

    /// <summary>
    /// Toggles the like status of a message, if it's already liked so it will unlike it
    /// </summary>
    /// <param name="messageId">The ID of the message to like or unlike.</param>
    /// <returns>HTTP status indicating the result of the like toggle operation.</returns>
    [Authorize]
    [HttpPost("messages/{messageId}/toggleLike")]
    public async Task<IActionResult> ToggleLike(int messageId)
    {
        return await this.requestProcessor.ProcessRequestAsync(messageId, async () =>
        {
            int userId = GetCurrentUserId(this.httpContextAccessor);
            await this.messageService.ToggleLike(userId, messageId);
            return this.NoContent();
        });
    }

    /// <summary>
    /// Retrieves the like count for a specific message.
    /// </summary>
    /// <param name="messageId">The ID of the message to get the like count for.</param>
    /// <returns>The total number of likes for the message.</returns>
    [HttpGet("messages/{messageId}/likes")]
    public async Task<IActionResult> GetLikeCount(int messageId)
    {
        return await this.requestProcessor.ProcessRequestAsync(messageId, async () =>
        {
            var messageModel = await this.messageService.GetByIdAsync(messageId);
            int likeCount = messageModel.LikesCounter;
            return this.Ok(likeCount);
        });
    }
}
