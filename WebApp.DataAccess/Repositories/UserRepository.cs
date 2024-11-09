using Microsoft.EntityFrameworkCore;
using WebApp.DataAccess.Data;
using WebApp.Infrastructure.Entities;
using WebApp.Infrastructure.Interfaces;
using WebApp.Infrastructure.Interfaces.IRepositories;

namespace WebApp.DataAccess.Repositories;

public class UserRepository : GenericRepository<User>, IUserRepository
{
    public UserRepository(ForumDbContext context)
        : base(context)
    {
    }

    public async Task<User> GetByEmailAsync(string email)
    {
        return await this.context.Users.Where(u => u.Email == email).FirstAsync();
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
