using Microsoft.EntityFrameworkCore;
using WebApp.Core.Entities;
using WebApp.Core.Interfaces;
using WebApp.Core.Interfaces.IRepositories;
using WebApp.DataAccess.Data;

namespace WebApp.DataAccess.Repositories;

public class TopicRepository : GenericRepository<Topic>, ITopicRepository
{
    public TopicRepository(ForumDbContext context)
        : base(context)
    {
    }

    public async Task<Topic> GetWithDetailsAsync(int id)
    {
        return await context.Topics
            .Include(t => t.Messages)
            .Include(t => t.User)
            .FirstAsync(t => t.Id == id);
    }

    public async Task<IEnumerable<Topic>> GetAllWithDetailsAsync()
    {
        return await context.Topics
            .Include(t => t.Messages)
            .Include(t => t.User)
            .ToListAsync();
    }
}
