namespace WebApp.Infrastructure.Interfaces.IRepositories;

using WebApp.Infrastructure.Entities;

public interface ITopicRepository : IRepository<Topic>
{
    public Task<Topic> GetWithDetailsAsync(int id);

    public Task<IEnumerable<Topic>> GetAllWithDetailsAsync();

    public Task<IEnumerable<Topic>> GetRangeAsync(int skip, int take);
}
