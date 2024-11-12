namespace WebApp.Infrastructure.Interfaces.IServices;

using WebApp.Infrastructure.Models.MessageModels;
using WebApp.Infrastructure.Models.TopicModels;

public interface IMessageService
{
    Task<IEnumerable<MessageBriefModel>> GetAllAsync(TopicQueryParametersModel? queryParameters = default);

    Task<MessageBriefModel> GetByIdAsync(params object[] keys);

    Task<int> AddAsync(MessageCreateModel model);

    Task UpdateAsync(MessageUpdateModel model);

    Task DeleteAsync(int modelId, int? ownerUserId = null);

    public Task LikeMessage(int userId, int messageId);

    public Task RemoveLike(int userId, int messageId);

    Task ToggleLike(int userId, int messageId);

    Task<IEnumerable<MessageBriefModel>> GetMessagesByTopicIdAsync(int topicId);

    public Task<MessageModel> GetByIdWithDetailsAsync(int id);

    public Task<bool> CheckMessageOwnerAsync(int messageId, int userId);
}
