using WebApp.Core.Entities;

namespace WebApp.Core.Interfaces.IRepositories;

public interface IMessageRepository
{
    public Task<Message> GetWithDetailsAsync(int id);

    public Task<IEnumerable<Message>> GetAllWithDetailsAsync();
}
