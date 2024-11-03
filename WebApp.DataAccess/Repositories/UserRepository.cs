using Microsoft.EntityFrameworkCore;
using WebApp.Core.Entities;
using WebApp.Core.Interfaces;
using WebApp.Core.Interfaces.IRepositories;
using WebApp.DataAccess.Data;

namespace WebApp.DataAccess.Repositories;

public class UserRepository : GenericRepository<User>, IUserRepository
{
    public UserRepository(ForumDbContext context)
        : base(context)
    {
    }

    public async Task<User> GetWithDetailsAsync(int id)
    {
        return await this.context.Users
            .Include(u => u.Topics)
            .Include(u => u.Messages)
            .Include(u => u.Reports)
            .FirstAsync(u => u.Id == id);
    }

    public async Task<IEnumerable<User>> GetAllWithDetailsAsync()
    {
        return await this.context.Users
            .Include(u => u.Topics)
            .Include(u => u.Messages)
            .Include(u => u.Reports)
            .ToListAsync();
    }
}
