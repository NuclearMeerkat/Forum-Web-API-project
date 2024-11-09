using WebApp.Core.Models.MessageModels;
using WebApp.Core.Models.TopicModels;

namespace WebApp.Core.Interfaces.IServices;
public interface IMessageService : ICrud<MessageBriefModel, MessageCreateModel, MessageUpdateModel, TopicQueryParametersModel, int>
{
    public Task LikeMessage(int userId, int messageId);

    public Task RemoveLike(int userId, int messageId);
    Task ToggleLike(int userId, int messageId);
    Task<IEnumerable<MessageBriefModel>> GetMessagesByTopicIdAsync(int topicId);
    public Task<MessageModel> GetByIdWithDetailsAsync(int id);
}
