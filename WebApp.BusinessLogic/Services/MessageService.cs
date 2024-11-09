using AutoMapper;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using Microsoft.Extensions.Primitives;
using WebApp.BusinessLogic.Validation;
using WebApp.Infrastructure.Entities;
using WebApp.Infrastructure.Interfaces.IRepositories;
using WebApp.Infrastructure.Interfaces.IServices;
using WebApp.Infrastructure.Models.MessageModels;
using WebApp.Infrastructure.Models.TopicModels;

namespace WebApp.BusinessLogic.Services;

public class MessageService : IMessageService
{
    private readonly IUnitOfWork unitOfWork;
    private readonly IMapper mapper;

    public MessageService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        this.unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    public async Task<IEnumerable<MessageBriefModel>> GetAllAsync(TopicQueryParametersModel queryParameters)
    {
        var messageEntities = await this.unitOfWork.MessageRepository.GetAllWithDetailsAsync();
        var messageModels = messageEntities.Select(m => this.mapper.MapWithExceptionHandling<MessageBriefModel>(m));

        return messageModels;
    }

    public async Task<MessageBriefModel> GetByIdAsync(params object[] keys)
    {
        var messageEntity = await this.unitOfWork.MessageRepository.GetByIdAsync(keys);
        var messageModel = this.mapper.MapWithExceptionHandling<MessageBriefModel>(messageEntity);

        return messageModel;
    }

    public async Task<MessageModel> GetByIdWithDetailsAsync(int id)
    {
        var messageEntity = await this.unitOfWork.MessageRepository.GetByIdWithDetailsAsync(id);
        var messageModel = this.mapper.MapWithExceptionHandling<MessageModel>(messageEntity);

        return messageModel;
    }


    public async Task<int> RegisterAsync(MessageCreateModel model)
    {
        ForumException.ThrowIfMessageCreateModelIsNotCorrect(model);

        if (!this.unitOfWork.UserRepository.IsExist(model.UserId))
        {
            throw new ForumException("User with this id does not exist");
        }

        if (!this.unitOfWork.TopicRepository.IsExist(model.TopicId))
        {
            throw new ForumException("Topic with this id does not exist");
        }

        var message = this.mapper.MapWithExceptionHandling<Message>(model);

        int messageId = (int)await this.unitOfWork.MessageRepository.AddAsync(message);
        await this.unitOfWork.SaveAsync();
        return messageId;
    }

    public async Task UpdateAsync(MessageUpdateModel model)
    {
        ForumException.ThrowIfNull(model);

        Message existingMessage;
        try
        {
            existingMessage = await this.unitOfWork.MessageRepository.GetByIdAsync(model.Id);
        }
        catch (Exception e)
        {
            throw new ForumException("Topic with this id is not found");
        }

        this.mapper.Map(model, existingMessage);
        this.unitOfWork.MessageRepository.Update(existingMessage);

        await this.unitOfWork.SaveAsync();
    }

    public async Task DeleteAsync(int modelId)
    {
        await this.unitOfWork.MessageRepository.DeleteByIdAsync(modelId);
        await this.unitOfWork.SaveAsync();
    }

    public async Task LikeMessage(int userId, int messageId)
    {
        await this.unitOfWork.MessageLikeRepository.AddAsync(
            new MessageLike { UserId = userId, MessageId = messageId, });

        await this.unitOfWork.SaveAsync();
    }

    public async Task RemoveLike(int userId, int messageId)
    {
        await this.unitOfWork.MessageLikeRepository.DeleteByIdAsync(userId, messageId);

        await this.unitOfWork.SaveAsync();
    }

    public async Task ToggleLike(int userId, int messageId)
    {
        if (!this.unitOfWork.MessageRepository.IsExist(messageId))
        {
            throw new ForumException("Message with this id does not exist");
        }

        if (!this.unitOfWork.UserRepository.IsExist(userId))
        {
            throw new ForumException("User with this id does not exist");
        }

        if (this.unitOfWork.MessageLikeRepository.IsExist(userId, messageId))
        {
            // Remove the like if it already exists
            await this.unitOfWork.MessageLikeRepository.DeleteByIdAsync(userId, messageId);
        }
        else
        {
            // Add like if none exists
            await this.unitOfWork.MessageLikeRepository.AddAsync(
                new MessageLike { UserId = userId, MessageId = messageId, });
        }

        await this.unitOfWork.SaveAsync();
    }

    public async Task<IEnumerable<MessageBriefModel>> GetMessagesByTopicIdAsync(int topicId)
    {
        var messages = await this.unitOfWork.MessageRepository.GetByTopicId(topicId);

        var messageModels = messages.Select(m => this.mapper.MapWithExceptionHandling<MessageBriefModel>(m));

        return messageModels;
    }
}
