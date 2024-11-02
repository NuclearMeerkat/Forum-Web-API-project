using WebApp.DataAccess.Entities;

namespace WebApp.DataAccess.Interfaces;

public interface IMessageRepository
{
    public Task<Message> GetWithDetailsAsync(int id);

    public Task<IEnumerable<Message>> GetAllWithDetailsAsync();
}
