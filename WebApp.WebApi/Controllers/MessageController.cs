using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApp.BusinessLogic.Validation;
using WebApp.Infrastructure.Interfaces.IServices;
using WebApp.Infrastructure.Models.MessageModels;

namespace WebApp.WebApi.Controllers;

[Route("api")]
[ApiController]
public class MessagesController : BaseController
{
    private readonly IMessageService messageService;
    private readonly IServiceProvider serviceProvider;
    private readonly IHttpContextAccessor httpContextAccessor;

    public MessagesController(
        IMessageService messageService,
        IServiceProvider serviceProvider,
        ITopicService topicService,
        IHttpContextAccessor httpContextAccessor)
    {
        ArgumentNullException.ThrowIfNull(messageService);
        ArgumentNullException.ThrowIfNull(serviceProvider);
        ArgumentNullException.ThrowIfNull(topicService);
        ArgumentNullException.ThrowIfNull(httpContextAccessor);
        this.messageService = messageService;
        this.serviceProvider = serviceProvider;
        this.httpContextAccessor = httpContextAccessor;
    }

    /// <summary>
    /// Retrieves a list of messages for a specific topic.
    /// </summary>
    /// <param name="topicId">The ID of the topic to retrieve messages for.</param>
    /// <returns>A list of messages associated with the specified topic.</returns>
    [HttpGet("topics/{topicId}/messages")]
    public async Task<IActionResult> GetMessagesForTopic(int topicId)
    {
        try
        {
            var messages = await this.messageService.GetMessagesByTopicIdAsync(topicId);
            return this.Ok(messages);
        }
        catch (ForumException e)
        {
            return this.NotFound(e.Message);
        }
    }

    /// <summary>
    /// Retrieves a message by its ID.
    /// </summary>
    /// <param name="id">The ID of the message to retrieve.</param>
    /// <returns>The details of the specified message.</returns>
    [HttpGet("messages/{id}")]
    public async Task<IActionResult> GetMessageById(int id)
    {
        MessageBriefModel message;
        try
        {
            message = await this.messageService.GetByIdAsync(id);
        }
        catch (ForumException e)
        {
            return this.NotFound(e.Message);
        }
        catch (InvalidOperationException)
        {
            return this.BadRequest($"Message with Id = {id} not found");
        }

        return this.Ok(message);
    }

    /// <summary>
    /// Creates a new message under a specific topic.
    /// </summary>
    /// <param name="topicId">The ID of the topic to associate the message with.</param>
    /// <param name="creationModel">The model containing the message details.</param>
    /// <returns>The created message with its details.</returns>
    [Authorize]
    [HttpPost("topics/{topicId}/messages")]
    public async Task<IActionResult> CreateMessage(int topicId, [FromBody] MessageCreateModel creationModel)
    {
        creationModel.TopicId = topicId;
        creationModel.UserId = this.GetCurrentUserId(this.httpContextAccessor);

        var validator = this.serviceProvider.GetService<IValidator<MessageCreateModel>>();

        return await this.ValidateAndExecuteAsync(creationModel, validator, async () =>
        {
            try
            {
                int messageId = await this.messageService.AddAsync(creationModel);
                return this.CreatedAtAction(nameof(this.GetMessageById), new { id = messageId }, creationModel);
            }
            catch (ForumException e)
            {
                return this.NotFound(e.Message);
            }
            catch (InvalidOperationException e)
            {
                return this.BadRequest(e.Message);
            }
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

        var validator = this.serviceProvider.GetService<IValidator<MessageUpdateModel>>();

        return await this.ValidateAndExecuteAsync(updateModel, validator, async () =>
        {
            try
            {
                int userId = this.GetCurrentUserId(this.httpContextAccessor);
                if (!await this.messageService.CheckMessageOwner(updateModel.Id, userId))
                {
                    return this.Forbid("You cannot update this message");
                }

                await this.messageService.UpdateAsync(updateModel);
                var updatedMessage = await this.messageService.GetByIdAsync(updateModel.Id);
                return this.Ok(updatedMessage);
            }
            catch (ForumException e)
            {
                return this.NotFound(e.Message);
            }
            catch (InvalidOperationException e)
            {
                return this.BadRequest(e.Message);
            }
        });
    }

    /// <summary>
    /// Deletes a message by its ID. Admins have permission to delete any message.
    /// </summary>
    /// <param name="id">The ID of the message to delete.</param>
    /// <returns>HTTP status indicating the result of the deletion.</returns>
    // [Authorize(Roles = "Admin,Moderator")]
    [HttpDelete("messages/Admin/{id:int}")]
    public async Task<IActionResult> AdminDeleteMessage(int id)
    {
        try
        {
            await this.messageService.DeleteAsync(id);
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
    /// Deletes the current user's message by its ID.
    /// </summary>
    /// <param name="id">The ID of the message to delete.</param>
    /// <returns>HTTP status indicating the result of the deletion.</returns>
    [HttpDelete("messages/{id:int}")]
    [Authorize]
    public async Task<IActionResult> DeleteMyMessage(int id)
    {
        try
        {
            int userId = this.GetCurrentUserId(this.httpContextAccessor);
            if (!await this.messageService.CheckMessageOwner(id, userId))
            {
                return this.Forbid("You cannot update this message");
            }

            await this.messageService.DeleteAsync(id);
            return this.NoContent();
        }
        catch (InvalidOperationException)
        {
            return this.NotFound($"Message with Id = {id} not found");
        }
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
        try
        {
            int userId = this.GetCurrentUserId(this.httpContextAccessor);
            if (!await this.messageService.CheckMessageOwner(messageId, userId))
            {
                return this.Forbid("You cannot update this message");
            }

            await this.messageService.ToggleLike(userId, messageId);
        }
        catch (ForumException e)
        {
            return this.BadRequest(e.Message);
        }

        return this.Ok();
    }

    /// <summary>
    /// Retrieves the like count for a specific message.
    /// </summary>
    /// <param name="messageId">The ID of the message to get the like count for.</param>
    /// <returns>The total number of likes for the message.</returns>
    [HttpGet("messages/{messageId}/likes")]
    public async Task<IActionResult> GetLikeCount(int messageId)
    {
        int likeCount;
        try
        {
            var messageModel = await this.messageService.GetByIdAsync(messageId);

            likeCount = messageModel.LikesCounter;
        }
        catch (ForumException e)
        {
            return this.NotFound(e.Message);
        }
        catch (InvalidOperationException ex)
        {
            return this.BadRequest(ex.Message);
        }

        return this.Ok(likeCount);
    }
}
