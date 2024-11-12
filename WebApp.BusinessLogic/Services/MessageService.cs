using AutoMapper;
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

#pragma warning disable CS8767 // Nullability of reference types in type of parameter doesn't match implicitly implemented member (possibly because of nullability attributes).
    public async Task<IEnumerable<MessageBriefModel>> GetAllAsync(TopicQueryParametersModel queryParameters)
#pragma warning restore CS8767 // Nullability of reference types in type of parameter doesn't match implicitly implemented member (possibly because of nullability attributes).
    {
        var messageEntities = await this.unitOfWork.MessageRepository.GetAllWithDetailsAsync();
        var messageModels = messageEntities.Select(m => this.mapper.MapWithExceptionHandling<MessageBriefModel>(m));

        return messageModels;
    }

    public async Task<MessageBriefModel> GetByIdAsync(params object[] keys)
    {
        if (!this.unitOfWork.MessageRepository.IsExist(keys))
        {
            throw new ForumException("This message does not exist");
        }

        var messageEntity = await this.unitOfWork.MessageRepository.GetByIdAsync(keys);
        var messageModel = this.mapper.MapWithExceptionHandling<MessageBriefModel>(messageEntity);

        return messageModel;
    }

    public async Task<MessageModel> GetByIdWithDetailsAsync(int id)
    {
        if (!this.unitOfWork.MessageRepository.IsExist(id))
        {
            throw new ForumException("The message does not exist");
        }

        var messageEntity = await this.unitOfWork.MessageRepository.GetByIdWithDetailsAsync(id);
        var messageModel = this.mapper.MapWithExceptionHandling<MessageModel>(messageEntity);

        return messageModel;
    }

    public async Task<int> AddAsync(MessageCreateModel model)
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

        if (!this.unitOfWork.MessageRepository.IsExist(model.Id))
        {
            throw new ForumException("Message with this id does not exist");
        }

        Message existingMessage;
        try
        {
            existingMessage = await this.unitOfWork.MessageRepository.GetByIdAsync(model.Id);
            if (existingMessage.UserId != model.UserId)
            {
                throw new UnauthorizedAccessException("This message does not belong to this user");
            }
        }
        catch (UnauthorizedAccessException ex)
        {
            throw new UnauthorizedAccessException(ex.Message);
        }
        catch (Exception e)
        {
            throw new InvalidOperationException(e.Message);
        }

        this.mapper.Map(model, existingMessage);
        this.unitOfWork.MessageRepository.Update(existingMessage);

        await this.unitOfWork.SaveAsync();
    }

    public async Task DeleteAsync(int modelId, int? ownerUserId = null)
    {
        if (ownerUserId is not null && !await this.CheckMessageOwnerAsync(modelId, (int)ownerUserId))
        {
            throw new UnauthorizedAccessException("This message does not belong to this user");
        }

        if (!this.unitOfWork.MessageRepository.IsExist(modelId))
        {
            throw new ForumException("Message with this id does not exist");
        }

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
        if (!this.unitOfWork.TopicRepository.IsExist(topicId))
        {
            throw new ForumException("Topic with this id does not exist");
        }

        var messages = await this.unitOfWork.MessageRepository.GetByTopicId(topicId);

        var messageModels = messages.Select(m => this.mapper.MapWithExceptionHandling<MessageBriefModel>(m));

        return messageModels;
    }

    public async Task<bool> CheckMessageOwnerAsync(int messageId, int userId)
    {
        Message message;
        try
        {
            message = await this.unitOfWork.MessageRepository.GetByIdAsync(messageId);
            if (message is null)
            {
                throw new InvalidOperationException();
            }
        }
        catch (InvalidOperationException)
        {
            throw new ForumException("Message with this id is not found");
        }

        if (userId == message.UserId)
        {
            return true;
        }

        return false;
    }
}
