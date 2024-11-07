using AutoMapper;
using WebApp.BusinessLogic.Validation;
using WebApp.Core.Entities;
using WebApp.Core.Interfaces.IRepositories;
using WebApp.Core.Interfaces.IServices;
using WebApp.Core.Models;

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

    public async Task<IEnumerable<MessageModel>> GetAllAsync()
    {
        var messageEntities = await this.unitOfWork.MessageRepository.GetAllWithDetailsAsync();
        var messageModels = messageEntities.Select(m => this.mapper.MapWithExceptionHandling<MessageModel>(m));

        return messageModels;
    }

    public async Task<MessageModel> GetByIdAsync(params object[] keys)
    {
        var messageEntity = await this.unitOfWork.MessageRepository.GetByIdAsync(keys);
        var messageModel = this.mapper.MapWithExceptionHandling<MessageModel>(messageEntity);

        return messageModel;
    }

    public async Task AddAsync(MessageCreateModel model)
    {
        ForumException.ThrowIfMessageCreateModelIsNotCorrect(model);

        var message = this.mapper.MapWithExceptionHandling<Message>(model);

        await this.unitOfWork.MessageRepository.AddAsync(message);
        await this.unitOfWork.SaveAsync();
    }

    public async Task UpdateAsync(MessageCreateModel model)
    {
        ForumException.ThrowIfNull(model);

        var message = this.mapper.MapWithExceptionHandling<Message>(model);
        this.unitOfWork.MessageRepository.Update(message);

        await this.unitOfWork.SaveAsync();
    }

    public async Task DeleteAsync(int modelId)
    {
        await this.unitOfWork.MessageRepository.DeleteByIdAsync(modelId);
        await this.unitOfWork.SaveAsync();
    }

    public async Task LikeMessage(int userId, int messageId)
    {
        await this.unitOfWork.MessageLikeRepository.AddAsync(new MessageLike
        {
            UserId = userId,
            MessageId = messageId,
        });

        await this.unitOfWork.SaveAsync();
    }

    public async Task RemoveLike(int userId, int messageId)
    {
        await this.unitOfWork.MessageLikeRepository.DeleteByIdAsync(userId, messageId);

        await this.unitOfWork.SaveAsync();
    }
}
