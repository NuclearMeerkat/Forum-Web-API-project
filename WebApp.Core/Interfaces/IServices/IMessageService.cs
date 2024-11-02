using WebApp.Core.Models;

namespace WebApp.Core.Interfaces.IServices;
public interface IMessageService : ICrud<MessageModel>
{
    public Task AddLike(int userId, int messageId);

    public Task RemoveLike(int userId, int messageId);
}
