using WebApp.Core.Models.MessageModels;
using WebApp.Core.Models.TopicModels;

namespace WebApp.Core.Interfaces.IServices;
public interface IMessageService : ICrud<MessageModel, MessageCreateModel, MessageUpdateModel, TopicQueryParametersModel, int>
{
    public Task LikeMessage(int userId, int messageId);

    public Task RemoveLike(int userId, int messageId);
}
