using WebApp.DataAccess.Entities;

namespace WebApp.DataAccess.Interfaces;

public interface ITopicRepository : IRepository<Topic>
{
    public Task<Topic> GetWithDetailsAsync(int id);

    public Task<IEnumerable<Topic>> GetAllWithDetailsAsync();
}
