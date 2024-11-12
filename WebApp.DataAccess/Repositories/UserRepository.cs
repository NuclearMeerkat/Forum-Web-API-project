using Microsoft.EntityFrameworkCore;
using WebApp.DataAccess.Data;
using WebApp.Infrastructure.Entities;
using WebApp.Infrastructure.Interfaces.IRepositories;

namespace WebApp.DataAccess.Repositories;

public class UserRepository : GenericRepository<User>, IUserRepository
{
    public UserRepository(ForumDbContext context)
        : base(context)
    {
    }

    public async Task<IEnumerable<User>> GetRange(int skip, int take)
    {
        return await this.context.Users.Skip(skip).Take(take).ToListAsync();
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
                .ThenInclude(m => m.Topic)
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

    public override async Task DeleteByIdAsync(params object[] keys)
    {
        var entity = await this.context.Set<User>().FindAsync(keys);
        if (entity != null)
        {
            var dependentTopicStars = this.context.TopicStars.Where(ts => ts.UserId == (int)keys[0]);
            var dependentReports = this.context.Reports.Where(ts => ts.UserId == (int)keys[0]);
            var dependentLikes = this.context.Likes.Where(ts => ts.UserId == (int)keys[0]);

            this.context.TopicStars.RemoveRange(dependentTopicStars);
            this.context.Reports.RemoveRange(dependentReports);
            this.context.Likes.RemoveRange(dependentLikes);

            _ = this.context.Set<User>().Remove(entity);
            _ = await this.context.SaveChangesAsync();
        }
    }
}
