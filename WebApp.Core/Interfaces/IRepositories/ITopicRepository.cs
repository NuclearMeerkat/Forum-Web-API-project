using WebApp.Core.Entities;

namespace WebApp.Core.Interfaces.IRepositories;

public interface ITopicRepository : IRepository<Topic>
{
    public Task<Topic> GetWithDetailsAsync(int id);

    public Task<IEnumerable<Topic>> GetAllWithDetailsAsync();
}
