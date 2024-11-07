using WebApp.Core.Models;

namespace WebApp.Core.Interfaces.IServices;
public interface IMessageService : ICrud<MessageModel, MessageCreateModel, int>
{
    public Task LikeMessage(int userId, int messageId);

    public Task RemoveLike(int userId, int messageId);
}
