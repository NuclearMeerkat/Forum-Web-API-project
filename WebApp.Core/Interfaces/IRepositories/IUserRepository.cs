using WebApp.Core.Entities;

namespace WebApp.Core.Interfaces.IRepositories;

public interface IUserRepository
{
    public Task<User> GetWithDetailsAsync(int id);

    public Task<IEnumerable<User>> GetAllWithDetailsAsync();
}
