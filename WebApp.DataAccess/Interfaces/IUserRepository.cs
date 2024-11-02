using WebApp.DataAccess.Entities;

namespace WebApp.DataAccess.Interfaces;

public interface IUserRepository
{
    public Task<User> GetWithDetailsAsync(int id);

    public Task<IEnumerable<User>> GetAllWithDetailsAsync();
}
