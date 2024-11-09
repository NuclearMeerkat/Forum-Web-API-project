using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApp.BusinessLogic.Validation;
using WebApp.Core.Entities;
using WebApp.Core.Interfaces.IServices;
using WebApp.Core.Models.MessageModels;

namespace WebApp.WebApi.Controllers;

[Route("api")]
[ApiController]
public class MessagesController : BaseController
{
    private readonly IMessageService messageService;
    private readonly IServiceProvider serviceProvider;
    private readonly ITopicService topicService;

    public MessagesController(IMessageService messageService, IServiceProvider serviceProvider,
        ITopicService topicService)
    {
        ArgumentNullException.ThrowIfNull(messageService);
        ArgumentNullException.ThrowIfNull(serviceProvider);
        ArgumentNullException.ThrowIfNull(topicService);
        this.messageService = messageService;
        this.serviceProvider = serviceProvider;
        this.topicService = topicService;
    }

    // GET: api/topics/{topicId}/messages
    [HttpGet("topics/{topicId}/messages")]
    public async Task<IActionResult> GetMessagesForTopic(int topicId)
    {
        var messages = await this.messageService.GetMessagesByTopicIdAsync(topicId);
        return this.Ok(messages);
    }

    // GET: api/messages/{id}
    [HttpGet("messages/{id}")]
    public async Task<IActionResult> GetMessageById(int id)
    {
        MessageModel message;
        try
        {
            message = await this.messageService.GetByIdWithDetailsAsync(id);
        }
        catch (InvalidOperationException)
        {
            return this.NotFound($"Message with Id = {id} not found");
        }

        return this.Ok(message);
    }

    // POST: api/topics/{topicId}/messages
    //[Authorize]
    [HttpPost("topics/{topicId}/messages")]
    public async Task<IActionResult> CreateMessage(int topicId, [FromBody] MessageCreateModel creationModel)
    {
        creationModel.TopicId = topicId;

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
                return this.BadRequest(e.Message);
            }
        });
    }

    // PUT: api/messages/{id}
    //[Authorize]
    [HttpPut("messages/{id:int}")]
    public async Task<IActionResult> UpdateMessage(int id, [FromBody] MessageUpdateModel updateModel)
    {
        updateModel.Id = id;

        var validator = this.serviceProvider.GetService<IValidator<MessageUpdateModel>>();

        return await this.ValidateAndExecuteAsync(updateModel, validator, async () =>
        {
            try
            {
                await this.messageService.UpdateAsync(updateModel);
                var updatedMessage = await this.messageService.GetByIdAsync(updateModel.Id);
                return this.Ok(updatedMessage);
            }
            catch (ForumException)
            {
                return this.NotFound();
            }
        });
    }

    // DELETE: api/messages/{id}
    //[Authorize(Roles = "Admin,Moderator")]
    [HttpDelete("messages/{id:int}")]
    public async Task<IActionResult> DeleteMessage(int id)
    {
        MessageBriefModel message;
        try
        {
            message = await this.messageService.GetByIdAsync(id);
        }
        catch (InvalidOperationException)
        {
            return this.NotFound($"Message with Id = {id} not found");
        }

        await this.messageService.DeleteAsync(id);
        return this.NoContent();
    }

    // POST: api/messages/{messageId}/likes
    //[Authorize]
    [HttpPost("messages/{messageId}/toggleLike")]
    public async Task<IActionResult> ToggleLike(int userId, int messageId)
    {
        //int userId = GetUserIdFromToken(); // Assume you retrieve user ID from JWT or session

        try
        {
            await this.messageService.ToggleLike(userId, messageId);
        }
        catch (ForumException e)
        {
            return this.BadRequest(e.Message);
        }

        return this.Ok();
    }

    // GET: api/messages/{messageId}/likes
    [HttpGet("messages/{messageId}/likes")]
    public async Task<IActionResult> GetLikeCount(int messageId)
    {
        int likeCount;
        try
        {
            var messageModel = await this.messageService.GetByIdAsync(messageId);

            likeCount = messageModel.LikesCounter;
        }
        catch (InvalidOperationException)
        {
            return this.NotFound($"Message with Id = {messageId} not found");
        }

        return this.Ok(likeCount);
    }
}
