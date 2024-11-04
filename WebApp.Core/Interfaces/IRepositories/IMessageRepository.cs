using WebApp.Core.Entities;

namespace WebApp.Core.Interfaces.IRepositories;

public interface IMessageRepository : IRepository<Message>
{
    public Task<Message> GetByIdWithDetailsAsync(int id);

    public Task<IEnumerable<Message>> GetAllWithDetailsAsync();
}
